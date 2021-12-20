Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsUserFields
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Dim _ID As Integer = 0
    Dim _UFID As Integer = 0
    Dim _UFTableID As Integer = 0
    Dim _UFName As String = ""
    Dim _UFValue As String = ""
    Dim _TableName As String = ""
    Dim _KeyValue As Integer = 0
    Dim _sErr As String = ""
    Dim _UserID As Integer = 0
    Dim _DataType As String = ""
    Dim _DataTypeID As Integer = 0

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Replace", cn)
    End Sub

    Public Sub Load()
        Try
            cn.Open()
            If _ID > 0 Then
                cm.CommandText = "Select dt.comboitem as UFDataType, dt.comboitemID as UFDataTypeID,v.UFValueID as ID, u.UFID, u.UFName, u.UFTableID, v.UFValue from t_UF_Tables t inner join t_UFields u on u.uftableid = t.uftableid inner join t_UF_Value v on v.ufid=u.ufid left outer join t_Comboitems dt on dt.comboitemid = u.UFDataType  where v.UFValueID = '" & _ID & "' order by u.UFName"
            ElseIf _UFID > 0 Then
                cm.CommandText = "Select u.UFDataType as UFDataTypeID, dt.comboitem as UFDataType,0 as ID, u.UFID, u.UFName, u.UFTableID, '' as UFValue from t_UF_Tables t inner join t_UFields u on u.uftableid = t.uftableid left outer join t_Comboitems dt on dt.comboitemid = u.UFDataType where u.UFID = '" & _UFID & "' order by u.UFName"
            Else
                cm.CommandText = "Select u.UFDataType, dt.comboitem as UFDataType,v.UFValueID as ID, u.UFID, u.UFName, u.UFTableID, v.UFValue from t_UF_Tables t inner join t_UFields u on u.uftableid = t.uftableid left outer join t_Comboitems dt on dt.comboitemid = u.UFDataType left outer join (select v.* from t_UF_Value v inner join t_UFields u on u.ufid = v.ufid inner join t_UF_Tables t on t.uftableid = u.uftableid where t.uftable = '" & _TableName & "' and v.keyvalue = " & _KeyValue & ") v on v.ufid=u.ufid where t.UFTable = '" & _TableName & "' order by u.UFName"
            End If

            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "UF")
            If ds.Tables("UF").Rows.Count > 0 Then
                dr = ds.Tables("UF").Rows(0)
                Fill_Values()
            End If
            cn.Close()
        Catch ex As Exception
            _sErr = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If _KeyValue > 0 Then
                ds.SelectCommand = "Select " & _KeyValue & " as KeyVal, case when v.UFValueID is null then 0 else v.UFValueID end as ID, u.UFID,u.UFDataType, u.UFName, u.UFTableID, v.UFValue from t_UF_Tables t inner join t_UFields u on u.uftableid = t.uftableid left outer join (select v.* from t_UF_Value v inner join t_UFields u on u.ufid = v.ufid inner join t_UF_Tables t on t.uftableid = u.uftableid where t.uftable = '" & _TableName & "' and v.keyvalue = " & _KeyValue & ") v on v.ufid=u.ufid where t.UFTable = '" & _TableName & "' order by u.UFName"
            Else
                ds.SelectCommand = "Select " & _KeyValue & " as KeyVal, case when v.UFValueID is null then 0 else v.UFValueID end as ID, u.UFID, u.UFDataType,u.UFName, u.UFTableID, v.UFValue from t_UF_Tables t inner join t_UFields u on u.uftableid = t.uftableid left outer join (select v.* from t_UF_Value v inner join t_UFields u on u.ufid = v.ufid inner join t_UF_Tables t on t.uftableid = u.uftableid where t.uftable = '" & _TableName & "' and v.keyvalue = " & _KeyValue & ") v on v.ufid=u.ufid where t.UFTable = '" & _TableName & "' order by u.UFName"
            End If
        Catch ex As Exception
            _sErr = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Fill_Values()
        _ID = IIf(dr("ID") Is System.DBNull.Value, 0, dr("ID"))
        _UFID = dr("UFID")
        _UFName = dr("UFName") & ""
        _UFTableID = IIf(dr("UFTableID") Is System.DBNull.Value, 0, dr("UFTableID"))
        _UFValue = dr("UFValue") & ""
        _DataType = dr("UFDataType") & ""
        _DataTypeID = IIf(dr("UFDataTypeID") Is System.DBNull.Value, 0, dr("UFDataTypeID"))
    End Sub

    Public Sub Save()
        cm.CommandText = "Select * from t_UF_Value where UFValueID = " & _ID
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim sqlCMD As New SqlCommandBuilder(da)
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            da.Fill(ds, "UF")
            If ds.Tables("UF").Rows.Count > 0 Then
                dr = ds.Tables("UF").Rows(0)
                Update_Field("UFValue", _UFValue, dr)
                dr("UFValue") = _UFValue
            Else
                dr = ds.Tables("UF").NewRow
                dr("UFID") = _UFID
                dr("KeyValue") = _KeyValue
                Update_Field("UFValue", _UFValue, dr)
                dr("UFValue") = _UFValue 'Update_Field("UFValue", _UFValue, dr)
                ds.Tables("UF").Rows.Add(dr)
            End If

            da.Update(ds, "UF")

        Catch ex As Exception
            _sErr = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            sqlCMD = Nothing

        End Try
    End Sub

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = _TableName & "ID"
            oEvents.KeyValue = _KeyValue
            oEvents.Create_Event()
            drow(sField) = sValue
            '_Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Public Sub Save_New_Field()
        cm.CommandText = "Select * from t_UFields where UFID = " & _UFID
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim sqlCMD As New SqlCommandBuilder(da)
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            da.Fill(ds, "UField")
            If ds.Tables("UField").Rows.Count > 0 Then
                dr = ds.Tables("UField").Rows(0)
            Else
                dr = ds.Tables("UField").NewRow
                dr("UFTableID") = _UFTableID
                dr("UFDataType") = _DataTypeID
                dr("UFName") = _UFName
                ds.Tables("UField").Rows.Add(dr)
            End If

            da.Update(ds, "UField")

        Catch ex As Exception
            _sErr = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            sqlCMD = Nothing

        End Try
    End Sub

    Private Sub Lookup_Table(ByVal bolName As Boolean)
        If bolName Then
            cm.CommandText = "Select * from t_UF_Tables where UFTable = '" & _TableName & "'"
        Else
            cm.CommandText = "Select * from t_UF_Tables where tableid = '" & _UFTableID & "'"
        End If
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If bolName Then
                    _UFTableID = dread("UFTableID")
                Else
                    _TableName = dread("UFTable")
                End If
            End If
            dread.Close()
            cn.Close()
        Catch ex As Exception
            _sErr = ex.ToString
        End Try

    End Sub

    Public Function Get_GroupID(ByVal gName As String) As Integer
        Dim uGroup As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select UFTableID from t_UF_Tables where UFTable = '" & gName & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                uGroup = dread("UFTableID")
            End If
            dread.Close()
        Catch ex As Exception
            _sErr = ex.Message
        Finally
            If cn.State <> ConnectionState.Open Then cn.Close()
        End Try
        Return uGroup
    End Function

    Public Function Get_UserFieldID(ByVal tableID As Integer, ByVal uField As String) As Integer
        Dim uFieldID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select UFID from t_UFields where uftableid = " & tableID & " and ufname = '" & uField & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                uFieldID = dread("UFID")
            End If
            dread.Close()
        Catch ex As Exception
            _sErr = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return uFieldID
    End Function

    Public Function Get_UserField_Value(ByVal UserFieldID As Integer, ByVal KV As Integer) As String
        Dim uFieldValue As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select UFValue from t_UF_VALUE where UFID = " & UserFieldID & " and KeyValue = " & KV & ""
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                uFieldValue = dread("UFValue")
            End If
            dread.Close()
        Catch ex As Exception
            _sErr = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return uFieldValue
    End Function

    Public Function Get_UserField_Value_ID(ByVal UserFieldID As Integer, ByVal KV As Integer) As Integer
        Dim uFieldValue As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select UFValueID from t_UF_VALUE where UFID = " & UserFieldID & " and KeyValue = " & KV & ""
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                uFieldValue = dread("UFValueID")
            End If
            dread.Close()
        Catch ex As Exception
            _sErr = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return uFieldValue
    End Function

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing

        MyBase.Finalize()
    End Sub

    Public ReadOnly Property Error_Message() As String
        Get
            Return _sErr
        End Get
    End Property

    Public Property DataTypeID() As Integer
        Get
            Return _DataTypeID
        End Get
        Set(ByVal value As Integer)
            _DataTypeID = value
        End Set
    End Property

    Public Property DataType() As String
        Get
            Return _DataType
        End Get
        Set(ByVal value As String)
            _DataType = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal value As String)
            If _TableName <> value Then
                _TableName = value
                Lookup_Table(True)
            End If
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UFID() As Integer
        Get
            Return _UFID
        End Get
        Set(ByVal value As Integer)
            _UFID = value
        End Set
    End Property

    Public Property TableID() As Integer
        Get
            Return _UFTableID
        End Get
        Set(ByVal value As Integer)
            If _UFTableID <> value Then
                _UFTableID = value
                Lookup_Table(False)
            End If
        End Set
    End Property

    Public Property FieldName() As String
        Get
            Return _UFName
        End Get
        Set(ByVal value As String)
            _UFName = value
        End Set
    End Property

    Public Property UFValue() As String
        Get
            Return _UFValue
        End Get
        Set(ByVal value As String)
            _UFValue = value
        End Set
    End Property

End Class
