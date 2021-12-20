Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsDoNotSellList
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _DateAdded As String = ""
    Dim _DateRemoved As String = ""
    Dim _AddedByID As Integer = 0
    Dim _RemovedByID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_DoNotSellList where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_DoNotSellList where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_DoNotSellList")
            If ds.Tables("t_DoNotSellList").Rows.Count > 0 Then
                dr = ds.Tables("t_DoNotSellList").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("DateRemoved") Is System.DBNull.Value) Then _DateRemoved = dr("DateRemoved")
        If Not (dr("AddedByID") Is System.DBNull.Value) Then _AddedByID = dr("AddedByID")
        If Not (dr("RemovedByID") Is System.DBNull.Value) Then _RemovedByID = dr("RemovedByID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DoNotSellList where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_DoNotSellList")
            If ds.Tables("t_DoNotSellList").Rows.Count > 0 Then
                dr = ds.Tables("t_DoNotSellList").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_DoNotSellListInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.datetime, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@DateRemoved", SqlDbType.datetime, 0, "DateRemoved")
                da.InsertCommand.Parameters.Add("@AddedByID", SqlDbType.int, 0, "AddedByID")
                da.InsertCommand.Parameters.Add("@RemovedByID", SqlDbType.int, 0, "RemovedByID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_DoNotSellList").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("DateRemoved", _DateRemoved, dr)
            Update_Field("AddedByID", _AddedByID, dr)
            Update_Field("RemovedByID", _RemovedByID, dr)
            If ds.Tables("t_DoNotSellList").Rows.Count < 1 Then ds.Tables("t_DoNotSellList").Rows.Add(dr)
            da.Update(ds, "t_DoNotSellList")
            _ID = ds.Tables("t_DoNotSellList").Rows(0).Item("ID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function Get_DNS_Entry(ByVal prosID As Integer) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ID from t_DoNotSellList where prospectID = " & prosID & " and DateRemoved Is Null"
            dread = cm.ExecuteReader
            If dread.hasrows Then
                dread.Read()
                ID = dread("ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Function Get_DNS_Entry_ByPhone(ByVal Phone As String) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select d.ID from t_DoNotSellList d where d.prospectID in (select ProspectID from t_ProspectPhone where number = '" & Phone & "') and DateRemoved Is Null"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ID = dread("ID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Function Get_Status(ByVal prosID As Integer) As String
        Dim status As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select top 1 * from t_DoNotSellList where prospectID = " & prosID & " order by ID desc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("RemovedByID") > 0 Then
                    status = "Add"
                Else
                    status = "Remove"
                End If
            Else
                status = "Add"
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return status
    End Function


    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property DateAdded() As String
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property DateRemoved() As String
        Get
            Return _DateRemoved
        End Get
        Set(ByVal value As String)
            _DateRemoved = value
        End Set
    End Property

    Public Property AddedByID() As Integer
        Get
            Return _AddedByID
        End Get
        Set(ByVal value As Integer)
            _AddedByID = value
        End Set
    End Property

    Public Property RemovedByID() As Integer
        Get
            Return _RemovedByID
        End Get
        Set(ByVal value As Integer)
            _RemovedByID = value
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

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
