Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRefurb2Item
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RefurbID As Integer = 0
    Dim _RefurbItemID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Refurb2Item where Refurb2ItemID = " & _ID, cn)
    End Sub

    Public Function List_Refurb_Items(ByVal RefurbID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select RefurbItemID as ID, i.Description, a.Comboitem as Area ,i.Active from t_RefurbItem i left outer join t_Comboitems a on a.comboitemid=i.areaid where RefurbItemid in (select Refurbitemid from t_Refurb2Item where RefurbID=" & RefurbID & ") order by description")
    End Function

    Public Sub Remove_Item(ByVal RefurbItemID As Integer, ByVal RefurbID As Integer)
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "delete from t_Refurb2Item where RefurbID=" & RefurbID & " and RefurbItemID = " & RefurbItemID
        cm.ExecuteNonQuery()
        If cn.State <> ConnectionState.Closed Then cn.Close()
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Refurb2Item where Refurb2ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Refurb2Item")
            If ds.Tables("t_Refurb2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_Refurb2Item").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RefurbID") Is System.DBNull.Value) Then _RefurbID = dr("RefurbID")
        If Not (dr("RefurbItemID") Is System.DBNull.Value) Then _RefurbItemID = dr("RefurbItemID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Refurb2Item where Refurb2ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Refurb2Item")
            If ds.Tables("t_Refurb2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_Refurb2Item").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Refurb2ItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RefurbID", SqlDbType.Int, 0, "RefurbID")
                da.InsertCommand.Parameters.Add("@RefurbItemID", SqlDbType.Int, 0, "RefurbItemID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Refurb2ItemID", SqlDbType.Int, 0, "Refurb2ItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Refurb2Item").NewRow
            End If
            Update_Field("RefurbID", _RefurbID, dr)
            Update_Field("RefurbItemID", _RefurbItemID, dr)
            If ds.Tables("t_Refurb2Item").Rows.Count < 1 Then ds.Tables("t_Refurb2Item").Rows.Add(dr)
            da.Update(ds, "t_Refurb2Item")
            _ID = ds.Tables("t_Refurb2Item").Rows(0).Item("Refurb2ItemID")
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
            oEvents.KeyField = "Refurb2ItemID"
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

    Public Property RefurbID() As Integer
        Get
            Return _RefurbID
        End Get
        Set(ByVal value As Integer)
            _RefurbID = value
        End Set
    End Property

    Public Property RefurbItemID() As Integer
        Get
            Return _RefurbItemID
        End Get
        Set(ByVal value As Integer)
            _RefurbItemID = value
        End Set
    End Property

    Public Property Refurb2ItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
