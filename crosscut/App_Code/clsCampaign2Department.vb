Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCampaign2Department
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CampaignID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _TourLocationID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Campaign2Department where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Campaign2Department where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Campaign2Department")
            If ds.Tables("t_Campaign2Department").Rows.Count > 0 Then
                dr = ds.Tables("t_Campaign2Department").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("CampaignID") Is System.DBNull.Value) Then _CampaignID = dr("CampaignID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("TourLocationID") Is System.DBNull.Value) Then _TourLocationID = dr("TourLocationID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Campaign2Department where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Campaign2Department")
            If ds.Tables("t_Campaign2Department").Rows.Count > 0 Then
                dr = ds.Tables("t_Campaign2Department").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Campaign2DepartmentInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.int, 0, "CampaignID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@TourLocationID", SqlDbType.int, 0, "TourLocationID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Campaign2Department").NewRow
            End If
            Update_Field("CampaignID", _CampaignID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("TourLocationID", _TourLocationID, dr)
            If ds.Tables("t_Campaign2Department").Rows.Count < 1 Then ds.Tables("t_Campaign2Department").Rows.Add(dr)
            da.Update(ds, "t_Campaign2Department")
            _ID = ds.Tables("t_Campaign2Department").Rows(0).Item("ID")
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

    Public Function List_Departments(ByVal campID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select c.ID, d.ComboItem as Department, tl.ComboItem as TourLocation from t_Campaign2Department c inner join t_ComboItems d on c.DepartmentID = d.ComboItemID inner join t_Comboitems tl on c.TourLocationID = tl.ComboitemID where c.CampaignID = " & campID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property CampaignID() As Integer
        Get
            Return _CampaignID
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
        End Set
    End Property

    Public Property DepartmentID() As Integer
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Integer)
            _DepartmentID = value
        End Set
    End Property

    Public Property TourLocationID() As Integer
        Get
            Return _TourLocationID
        End Get
        Set(ByVal value As Integer)
            _TourLocationID = value
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

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
End Class
