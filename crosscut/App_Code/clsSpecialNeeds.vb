Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSpecialNeeds
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _NeedID As Integer = 0
    Dim _NeedText As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SpecialNeeds where SpecialNeedID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SpecialNeeds where SpecialNeedID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SpecialNeeds")
            If ds.Tables("t_SpecialNeeds").Rows.Count > 0 Then
                dr = ds.Tables("t_SpecialNeeds").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
        If Not (dr("NeedID") Is System.DBNull.Value) Then _NeedID = dr("NeedID")
        If Not (dr("NeedText") Is System.DBNull.Value) Then _NeedText = dr("NeedText")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SpecialNeeds where SpecialNeedID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SpecialNeeds")
            If ds.Tables("t_SpecialNeeds").Rows.Count > 0 Then
                dr = ds.Tables("t_SpecialNeeds").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SpecialNeedsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@NeedID", SqlDbType.int, 0, "NeedID")
                da.InsertCommand.Parameters.Add("@NeedText", SqlDbType.varchar, 0, "NeedText")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SpecialNeedID", SqlDbType.Int, 0, "SpecialNeedID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SpecialNeeds").NewRow
            End If
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("NeedID", _NeedID, dr)
            Update_Field("NeedText", _NeedText, dr)
            If ds.Tables("t_SpecialNeeds").Rows.Count < 1 Then ds.Tables("t_SpecialNeeds").Rows.Add(dr)
            da.Update(ds, "t_SpecialNeeds")
            _ID = ds.Tables("t_SpecialNeeds").Rows(0).Item("SpecialNeedID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "SpecialNeedID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function List() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("SpecialNeedID")
        dt.Columns.Add("SpecialNeed")
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select sn.*, ci.ComboItem as spNeed from t_SpecialNeeds sn inner join t_ComboItems ci on sn.NeedID = ci.ComboItemID where sn.KeyField = '" & _KeyField & "' and sn.KeyValue = '" & _KeyValue & "'"
        dread = cm.ExecuteReader
        Do While dread.Read
            dr = dt.NewRow
            dr("SpecialNeedID") = dread("SpecialNeedID")
            If dread("spNeed") = "Other" Then
                dr("SpecialNeed") = dread("NeedText")
            Else
                dr("SpecialNeed") = dread("spNeed")
            End If
            dt.Rows.Add(dr)
        Loop
        'Catch ex As Exception
        '_Err = ex.Message
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return dt
    End Function
    Public Function List_SpecialNeeds() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select ci.ComboItemID, ci.ComboItem from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName = 'SpecialNeeds' and ci.Active = '1'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
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

    Public Property NeedID() As Integer
        Get
            Return _NeedID
        End Get
        Set(ByVal value As Integer)
            _NeedID = value
        End Set
    End Property

    Public Property NeedText() As String
        Get
            Return _NeedText
        End Get
        Set(ByVal value As String)
            _NeedText = value
        End Set
    End Property

    Public Property SpecialNeedID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
