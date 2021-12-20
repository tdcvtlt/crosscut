Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRefurbHist2Item
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RefurbHistId As Integer = 0
    Dim _RefurbItemID As Integer = 0
    Dim _Checked As Boolean = False
    Dim _CheckedById As Integer = 0
    Dim _DateChecked As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Refurb2Item where RefurbHist2Item = " & _ID, cn)
    End Sub

    Public Function List(RefurbHistID As Integer) As DataTable
        cm.CommandText = "select h.refurbHist2Item,h.Checked, a.comboitem as Area, i.Description, p.UserName as CheckedBy, h.DateChecked from t_RefurbHist2Item h inner join t_RefurbItem i on i.RefurbItemID=h.RefurbItemID left outer join t_ComboItems a on a.ComboItemID=i.AreaID left outer join t_Personnel p on p.PersonnelID=h.CheckedById where h.RefurbHistId = " & RefurbHistID & " order by a.ComboItem, i.Description"
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim ret As New DataTable
        Try
            da.Fill(ds, "QC")
            ret = ds.Tables("QC")
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cm = Nothing
            cn = Nothing
            da = Nothing
            ds = Nothing
        End Try
        Return ret
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Refurb2Item where RefurbHist2Item = " & _ID
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
        If Not (dr("RefurbHistId") Is System.DBNull.Value) Then _RefurbHistId = dr("RefurbHistId")
        If Not (dr("RefurbItemID") Is System.DBNull.Value) Then _RefurbItemID = dr("RefurbItemID")
        If Not (dr("Checked") Is System.DBNull.Value) Then _Checked = dr("Checked")
        If Not (dr("CheckedById") Is System.DBNull.Value) Then _CheckedById = dr("CheckedById")
        If Not (dr("DateChecked") Is System.DBNull.Value) Then _DateChecked = dr("DateChecked")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Refurb2Item where RefurbHist2Item = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Refurb2Item")
            If ds.Tables("t_Refurb2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_Refurb2Item").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Refurb2ItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RefurbHistId", SqlDbType.Int, 0, "RefurbHistId")
                da.InsertCommand.Parameters.Add("@RefurbItemID", SqlDbType.Int, 0, "RefurbItemID")
                da.InsertCommand.Parameters.Add("@Checked", SqlDbType.Bit, 0, "Checked")
                da.InsertCommand.Parameters.Add("@CheckedById", SqlDbType.Int, 0, "CheckedById")
                da.InsertCommand.Parameters.Add("@DateChecked", SqlDbType.DateTime, 0, "DateChecked")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RefurbHist2Item", SqlDbType.Int, 0, "RefurbHist2Item")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Refurb2Item").NewRow
            End If
            Update_Field("RefurbHistId", _RefurbHistId, dr)
            Update_Field("RefurbItemID", _RefurbItemID, dr)
            Update_Field("Checked", _Checked, dr)
            Update_Field("CheckedById", _CheckedById, dr)
            Update_Field("DateChecked", _DateChecked, dr)
            If ds.Tables("t_Refurb2Item").Rows.Count < 1 Then ds.Tables("t_Refurb2Item").Rows.Add(dr)
            da.Update(ds, "t_Refurb2Item")
            _ID = ds.Tables("t_Refurb2Item").Rows(0).Item("RefurbHist2Item")
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
            oEvents.KeyField = "RefurbHist2Item"
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

    Public Property RefurbHistId() As Integer
        Get
            Return _RefurbHistId
        End Get
        Set(ByVal value As Integer)
            _RefurbHistId = value
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

    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
        End Set
    End Property

    Public Property CheckedById() As Integer
        Get
            Return _CheckedById
        End Get
        Set(ByVal value As Integer)
            _CheckedById = value
        End Set
    End Property

    Public Property DateChecked() As String
        Get
            Return _DateChecked
        End Get
        Set(ByVal value As String)
            _DateChecked = value
        End Set
    End Property

    Public Property RefurbHist2Item() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
