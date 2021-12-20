Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsTourLetters
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Description As String = ""
    Dim _LetterID As Integer = 0
    Dim _Subject As String = ""
    Dim _EmailAddress As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_TourLetters where TourLetterID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_TourLetters where TourLetterID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_TourLetters")
            If ds.Tables("t_TourLetters").Rows.Count > 0 Then
                dr = ds.Tables("t_TourLetters").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub


    Public Function Search_Letters(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "" Then
                ds.SelectCommand = "Select tourletterid as ID, description from t_tourletters order by tourletterid"
            Else
                ds.SelectCommand = String.Format("Select tourletterid as ID, description from t_tourletters where r.Description like '{0}%' order by tourletterid", filter)
            End If
        Catch ex As Exception
            _Err = ex.Message
            HttpContext.Current.Response.Write(ex.Message)
        End Try
        Return ds
    End Function


    Public Function Get_Letters(campaignID As Int32, locationID As Int32) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = String.Format("SELECT * " & _
                                "FROM t_TourLetter2Campaign c inner join t_TourLetter2Location l on c.TourLetterID = l.TourLetterID " & _
                                "inner join t_Letters t on t.LetterID = c.TourLetterID inner join t_TourLetters tl on tl.LetterID = t.LetterID " & _
                                "where c.CampaignID in ({1}) and l.TourLocationID in ({0}) and c.Active = 1 and l.Active = 1", locationID, campaignID)
        Catch ex As Exception
            _Err = ex.Message
            HttpContext.Current.Response.Write(ex.Message)
        End Try
        Return ds
    End Function

    Public Function Get_Email(tourID As Int32) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = String.Format("select e.Email from t_ProspectEmail e  " & _
                                "inner join t_Prospect p on p.ProspectID = e.ProspectID inner join t_Tour t on t.prospectid = p.ProspectID " & _
                                "where t.TourID = {0} and e.IsActive=1 and e.IsPrimary=1", tourID)
        Catch ex As Exception
            _Err = ex.Message
            HttpContext.Current.Response.Write(ex.Message)
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("LetterID") Is System.DBNull.Value) Then _LetterID = dr("LetterID")
        If Not (dr("Subject") Is System.DBNull.Value) Then _Subject = dr("Subject")
        If Not (dr("EmailAddress") Is System.DBNull.Value) Then _EmailAddress = dr("EmailAddress")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_TourLetters where TourLetterID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_TourLetters")
            If ds.Tables("t_TourLetters").Rows.Count > 0 Then
                dr = ds.Tables("t_TourLetters").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_TourLettersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.NChar, 50, "Description")
                da.InsertCommand.Parameters.Add("@LetterID", SqlDbType.Int, 8, "LetterID")
                da.InsertCommand.Parameters.Add("@Subject", SqlDbType.VarChar, 50, "Subject")
                da.InsertCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50, "EmailAddress")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TourLetterID", SqlDbType.Int, 8, "TourLetterID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_TourLetters").NewRow
            End If
            Update_Field("Description", _Description, dr)
            Update_Field("LetterID", _LetterID, dr)
            Update_Field("Subject", _Subject, dr)
            Update_Field("EmailAddress", _EmailAddress, dr)
            If ds.Tables("t_TourLetters").Rows.Count < 1 Then ds.Tables("t_TourLetters").Rows.Add(dr)
            da.Update(ds, "t_TourLetters")
            _ID = ds.Tables("t_TourLetters").Rows(0).Item("TourLetterID")

            Return True
        Catch ex As Exception
            _Err = ex.ToString
            HttpContext.Current.Response.Write(ex.Message)
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField).ToString(), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "TourLetterID"
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property LetterID() As Integer
        Get
            Return _LetterID
        End Get
        Set(ByVal value As Integer)
            _LetterID = value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property

    Public Property EmailAddress() As String
        Get
            Return _EmailAddress
        End Get
        Set(ByVal value As String)
            _EmailAddress = value
        End Set
    End Property

    Public Property TourLetterID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
