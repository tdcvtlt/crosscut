Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsReservationLetters
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Description As String = ""
    Dim _ResortCompanyID As Integer = 0
    Dim _LetterText As String = ""
    Dim _Subject As String = ""
    Dim _BookedTour As Boolean = False
    Dim _EmailAddress As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ReservationLetters where ReservationLetterID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ReservationLetters where ReservationLetterID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ReservationLetters")
            If ds.Tables("t_ReservationLetters").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationLetters").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("ResortCompanyID") Is System.DBNull.Value) Then _ResortCompanyID = dr("ResortCompanyID")
        If Not (dr("LetterText") Is System.DBNull.Value) Then _LetterText = dr("LetterText")
        If Not (dr("BookedTour") Is System.DBNull.Value) Then _BookedTour = dr("BookedTour")
        If Not (dr("Subject") Is System.DBNull.Value) Then _Subject = dr("Subject")
        If Not (dr("EmailAddress") Is System.DBNull.Value) Then _EmailAddress = dr("EmailAddress")

    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ReservationLetters where ReservationLetterID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ReservationLetters")
            If ds.Tables("t_ReservationLetters").Rows.Count > 0 Then
                dr = ds.Tables("t_ReservationLetters").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ReservationLettersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@ResortCompanyID", SqlDbType.Int, 0, "ResortCompanyID")
                da.InsertCommand.Parameters.Add("@LetterText", SqlDbType.text, 0, "LetterText")
                da.InsertCommand.Parameters.Add("@BookedTour", SqlDbType.bit, 0, "BookedTour")
                da.InsertCommand.Parameters.Add("@Subject", SqlDbType.VarChar, 0, "Subject")
                da.InsertCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 0, "EmailAddress")

                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReservationLetterID", SqlDbType.Int, 0, "ReservationLetterID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ReservationLetters").NewRow
            End If
            Update_Field("Description", _Description, dr)
            Update_Field("ResortCompanyID", _ResortCompanyID, dr)
            Update_Field("LetterText", _LetterText, dr)
            Update_Field("BookedTour", _BookedTour, dr)
            Update_Field("Subject", _Subject, dr)
            Update_Field("EmailAddress", _EmailAddress, dr)
            If ds.Tables("t_ReservationLetters").Rows.Count < 1 Then ds.Tables("t_ReservationLetters").Rows.Add(dr)
            da.Update(ds, "t_ReservationLetters")
            _ID = ds.Tables("t_ReservationLetters").Rows(0).Item("ReservationLetterID")
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
            oEvents.KeyField = "ReservationLetterID"
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

    Public Function Search_Letters(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "" Then
                ds.SelectCommand = "Select r.ReservationLetterID as ID, r.Description, rc.COmboItem as Company from t_ReservationLetters r left outer join t_ComboItems rc on r.ResortCompanyID = rc.ComboItemID order by ReservationLetterID"
            Else
                ds.SelectCommand = "Select r.ReservationLetterID as ID, r.Description, rc.COmboItem as Company from t_ReservationLetters r left outer join t_ComboItems rc on r.ResortCompanyID = rc.ComboItemID where r.Description like '" & filter & "%' order by ReservationLetterID"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Find_Letter(ByVal resID As Integer) As Integer
        Dim letterID As Integer = 0
        Try
            Dim oRes As New clsReservations
            Dim oCombo As New clsComboItems
            Dim oTour As New clsTour
            oRes.ReservationID = resID
            oRes.Load()
            If cn.State <> ConnectionState.Open Then cn.Open()
            If oCombo.Lookup_ComboItem(oRes.ResLocationID) = "KCP" Then
                If oTour.Get_Tour_By_Res(oRes.ReservationID) > 0 Then
                    oTour.TourID = oTour.Get_Tour_By_Res(oRes.ReservationID)
                    oTour.Load()
                    If InStr(oCombo.Lookup_ComboItem(oTour.StatusID), "Cancel") > 0 Then
                        cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Source rl on r.ReservationLetterID = rl.ResLetterID inner join t_ReservationLetter2Location rlo on r.ReservationLetterID = rlo.ResLetterID where r.BookedTour = 0 and rl.SourceID = " & oRes.SourceID & " and rlo.ResLocationID = " & oRes.ResLocationID & " and rl.Active = 1 and rlo.Active = 1"
                    Else
                        cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Source rl on r.ReservationLetterID = rl.ResLetterID inner join t_ReservationLetter2Location rlo on r.ReservationLetterID = rlo.ResLetterID where r.BookedTour = 1 and rl.SourceID = " & oRes.SourceID & " and rlo.ResLocationID = " & oRes.ResLocationID & " and rl.Active = 1 and rlo.Active = 1"
                    End If
                Else
                    cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Source rl on r.ReservationLetterID = rl.ResLetterID inner join t_ReservationLetter2Location rlo on r.ReservationLetterID = rlo.ResLetterID where r.BookedTour = 0 and rl.SourceID = " & oRes.SourceID & " and rlo.ResLocationID = " & oRes.ResLocationID & " and rl.Active = 1 and rlo.Active = 1"
                    'cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Source rl on r.ReservationLetterID = rl.ResLetterID where r.BookedTour = 0 and rl.SourceID = " & oRes.SourceID
                End If
            Else
                If oTour.Get_Tour_By_Res(oRes.ReservationID) > 0 Then
                    oTour.TourID = oTour.Get_Tour_By_Res(oRes.ReservationID)
                    oTour.Load()
                    If InStr(oCombo.Lookup_ComboItem(oTour.StatusID), "Cancel") > 0 Then
                        cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Location rl on r.ReservationLetterID = rl.ResLetterID where r.BookedTour = 0 and rl.ResLocationID = " & oRes.ResLocationID & " and r.ResortCompanyID = " & oRes.ResortCompanyID & " and rl.Active = 1"
                    Else
                        cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Location rl on r.ReservationLetterID = rl.ResLetterID where r.BookedTour = 1 and rl.ResLocationID = " & oRes.ResLocationID & " and r.ResortCompanyID = " & oRes.ResortCompanyID & " and rl.Active = 1"
                    End If
                Else
                    cm.CommandText = "Select ReservationLetterID from t_ReservationLetters r inner join t_ReservationLetter2Location rl on r.ReservationLetterID = rl.ResLetterID where r.BookedTour = 0 and rl.ResLocationID = " & oRes.ResLocationID & " and r.ResortCompanyID = " & oRes.ResortCompanyID & " and rl.Active = 1"
                End If
            End If
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                letterID = dread("ReservationLetterID")
            End If
            dread.Close()
            oRes = Nothing
            oCombo = Nothing
            oTour = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return letterID
    End Function

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property ResortCompanyID() As Integer
        Get
            Return _ResortCompanyID
        End Get
        Set(ByVal value As Integer)
            _ResortCompanyID = value
        End Set
    End Property



    Public Property LetterText() As String
        Get
            Return _LetterText
        End Get
        Set(ByVal value As String)
            _LetterText = value
        End Set
    End Property

    Public Property BookedTour() As Boolean
        Get
            Return _BookedTour
        End Get
        Set(ByVal value As Boolean)
            _BookedTour = value
        End Set
    End Property

    Public Property ReservationLetterID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
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

End Class
