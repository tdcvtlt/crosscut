Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsQuickCheckHist2Item
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _QuickCheckHistId As Integer = 0
    Dim _QuickCheckItemID As Integer = 0
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
        cm = New SqlCommand("Select * from t_QuickCheckHist2Item where QuickCheckHist2Item = " & _ID, cn)
    End Sub

    Public Function List(QuickCheckHistID As Integer) As DataTable
        cm.CommandText = "select h.QuickCheckHist2Item,h.Checked, a.comboitem as Area, i.Description, p.UserName as CheckedBy, h.DateChecked from t_QuickCheckHist2Item h inner join t_QuickcheckItem i on i.QuickCheckItemID=h.QuickCheckItemID left outer join t_ComboItems a on a.ComboItemID=i.AreaID left outer join t_Personnel p on p.PersonnelID=h.CheckedById where h.QuickCheckHistId = " & QuickCheckHistID & " order by a.ComboItem, i.Description"
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
            cm.CommandText = "Select * from t_QuickCheckHist2Item where QuickCheckHist2Item = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_QuickCheckHist2Item")
            If ds.Tables("t_QuickCheckHist2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_QuickCheckHist2Item").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("QuickCheckHist2Item") Is System.DBNull.Value) Then _ID = dr("QuickCheckHist2Item")
        If Not (dr("QuickCheckHistId") Is System.DBNull.Value) Then _QuickCheckHistId = dr("QuickCheckHistId")
        If Not (dr("QuickCheckItemID") Is System.DBNull.Value) Then _QuickCheckItemID = dr("QuickCheckItemID")
        If Not (dr("Checked") Is System.DBNull.Value) Then _Checked = dr("Checked")
        If Not (dr("CheckedById") Is System.DBNull.Value) Then _CheckedById = dr("CheckedById")
        If Not (dr("DateChecked") Is System.DBNull.Value) Then _DateChecked = dr("DateChecked")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_QuickCheckHist2Item where QuickCheckHist2Item = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_QuickCheckHist2Item")
            If ds.Tables("t_QuickCheckHist2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_QuickCheckHist2Item").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_QuickCheckHist2ItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@QuickCheckHistId", SqlDbType.Int, 0, "QuickCheckHistId")
                da.InsertCommand.Parameters.Add("@QuickCheckItemID", SqlDbType.Int, 0, "QuickCheckItemID")
                da.InsertCommand.Parameters.Add("@Checked", SqlDbType.Bit, 0, "Checked")
                da.InsertCommand.Parameters.Add("@CheckedById", SqlDbType.Int, 0, "CheckedById")
                da.InsertCommand.Parameters.Add("@DateChecked", SqlDbType.DateTime, 0, "DateChecked")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@QuickCheckHist2Item", SqlDbType.Int, 0, "QuickCheckHist2Item")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_QuickCheckHist2Item").NewRow
            End If
            Update_Field("QuickCheckHistId", _QuickCheckHistId, dr)
            Update_Field("QuickCheckItemID", _QuickCheckItemID, dr)
            Update_Field("Checked", _Checked, dr)
            Update_Field("CheckedById", _CheckedById, dr)
            Update_Field("DateChecked", _DateChecked, dr)
            If ds.Tables("t_QuickCheckHist2Item").Rows.Count < 1 Then ds.Tables("t_QuickCheckHist2Item").Rows.Add(dr)
            da.Update(ds, "t_QuickCheckHist2Item")
            _ID = ds.Tables("t_QuickCheckHist2Item").Rows(0).Item("QuickCheckHist2Item")
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
            oEvents.KeyField = "QuickCheckHist2Item"
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

    Public Property QuickCheckHistId() As Integer
        Get
            Return _QuickCheckHistId
        End Get
        Set(ByVal value As Integer)
            _QuickCheckHistId = value
        End Set
    End Property

    Public Property QuickCheckItemID() As Integer
        Get
            Return _QuickCheckItemID
        End Get
        Set(ByVal value As Integer)
            _QuickCheckItemID = value
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

    Public Property QuickCheckHist2Item() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
