Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRefurbItem
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Description As String = ""
    Dim _AreaID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RefurbItem where RefurbItemID = " & _ID, cn)
    End Sub

    Public Function List() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select i.*, coalesce(a.comboitem, 'All') + ' - ' + i.Description as ItemAreaDescription from t_RefurbItem i left outer join t_Comboitems a on a.comboitemid=i.areaid where i.Active = 1 order by a.Comboitem, i.Description")
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RefurbItem where RefurbItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RefurbItem")
            If ds.Tables("t_RefurbItem").Rows.Count > 0 Then
                dr = ds.Tables("t_RefurbItem").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("AreaID") Is System.DBNull.Value) Then _AreaID = dr("AreaID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RefurbItem where RefurbItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RefurbItem")
            If ds.Tables("t_RefurbItem").Rows.Count > 0 Then
                dr = ds.Tables("t_RefurbItem").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RefurbItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@AreaID", SqlDbType.Int, 0, "AreaID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RefurbItemID", SqlDbType.Int, 0, "RefurbItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RefurbItem").NewRow
            End If
            Update_Field("Description", _Description, dr)
            Update_Field("AreaID", _AreaID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_RefurbItem").Rows.Count < 1 Then ds.Tables("t_RefurbItem").Rows.Add(dr)
            da.Update(ds, "t_RefurbItem")
            _ID = ds.Tables("t_RefurbItem").Rows(0).Item("RefurbItemID")
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
            oEvents.KeyField = "RefurbItemID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property AreaID() As Integer
        Get
            Return _AreaID
        End Get
        Set(ByVal value As Integer)
            _AreaID = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property RefurbItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
