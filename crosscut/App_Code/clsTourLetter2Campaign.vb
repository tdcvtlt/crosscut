Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsTourLetter2Campaign
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TourLetterID As Integer = 0
    Dim _CampaignID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_TourLetter2Campaign where TourLetter2CampaignID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_TourLetter2Campaign where TourLetter2CampaignID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_TourLetter2Campaign")
            If ds.Tables("t_TourLetter2Campaign").Rows.Count > 0 Then
                dr = ds.Tables("t_TourLetter2Campaign").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select * from t_TourLetter2Campaign where tourletterid =" & TourLetterID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("TourLetterID") Is System.DBNull.Value) Then _TourLetterID = dr("TourLetterID")
        If Not (dr("CampaignID") Is System.DBNull.Value) Then _CampaignID = dr("CampaignID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_TourLetter2Campaign where TourLetter2CampaignID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_TourLetter2Campaign")
            If ds.Tables("t_TourLetter2Campaign").Rows.Count > 0 Then
                dr = ds.Tables("t_TourLetter2Campaign").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_TourLetter2CampaignInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TourLetterID", SqlDbType.int, 0, "TourLetterID")
                da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.int, 0, "CampaignID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TourLetter2CampaignID", SqlDbType.Int, 0, "TourLetter2CampaignID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_TourLetter2Campaign").NewRow
            End If
            Update_Field("TourLetterID", _TourLetterID, dr)
            Update_Field("CampaignID", _CampaignID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_TourLetter2Campaign").Rows.Count < 1 Then ds.Tables("t_TourLetter2Campaign").Rows.Add(dr)
            da.Update(ds, "t_TourLetter2Campaign")
            _ID = ds.Tables("t_TourLetter2Campaign").Rows(0).Item("TourLetter2CampaignID")
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
            oEvents.KeyField = "TourLetter2CampaignID"
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

    Public Property TourLetterID() As Integer
        Get
            Return _TourLetterID
        End Get
        Set(ByVal value As Integer)
            _TourLetterID = value
        End Set
    End Property

    Public Property CampaignID() As Integer
        Get
            Return _CampaignID
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
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

    Public Property TourLetter2CampaignID As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Campaigns As SqlDataSource
        Get            
            Return New SqlDataSource(Resources.Resource.cns, "Select * from t_Campaign where active = 1 order by name")
        End Get
    End Property

End Class
