Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProspect
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FName As String = ""
    Dim _MiddleInit As String = ""
    Dim _LName As String = ""
    Dim _Dear As Integer = 0
    Dim _ProspectNumber As String = ""
    Dim _CompanyName As String = ""
    Dim _Title As String = ""
    Dim _Birthdate As String = ""
    Dim _MaritalStatusID As Integer = 0
    Dim _SpouseID As Integer = 0
    Dim _SourceID As Integer = 0
    Dim _ReferringProspectID As Integer = 0
    Dim _DateReferred As String = ""
    Dim _AssignedRepID As Integer = 0
    Dim _FedDNCFlag As Boolean = False
    Dim _SSN As String = ""
    Dim _IsOwner As Boolean = False
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _Status As Integer = 0
    Dim _DriverLicense As String = ""
    Dim _DriverLicenseStateID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Income As Decimal = 0
    Dim _IncomeDebt As Decimal = 0
    Dim _CreditScore As Decimal = 0
    Dim _Occupation As Integer = 0
    Dim _SpouseFirstName As String = ""
    Dim _SpouseLastName As String = ""
    Dim _SpouseSSN As String = ""
    Dim _SpouseCreditScore As Integer = 0
    Dim _AnniversaryDate As String = ""
    Dim _SpouseBirthDate As String = ""
    Dim _ClubFeeStatusID As Integer = 0
    Dim _Err As String = ""


    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Prospect where prospectid = " & _ID, cn)

    End Sub

    Public Sub Load_By_LastName()
        Try
            cm.CommandText = "Select *, case when (select count(*) from t_Contract where prospectid = " & _ID & ") > 0 then 1 else 0 end as OwnerFlag from t_Prospect where lastname = '" & _LName & "'"
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Prospect")
            '_Err = ds.Tables("Prospect").Rows.Count
            If ds.Tables("Prospect").Rows.Count > 0 Then
                dr = ds.Tables("Prospect").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select *, case when (select count(*) from t_Contract where prospectid = " & _ID & ") > 0 then 1 else 0 end as OwnerFlag from t_Prospect where prospectid = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Prospect")
            '_Err = ds.Tables("Prospect").Rows.Count
            If ds.Tables("Prospect").Rows.Count > 0 Then
                dr = ds.Tables("Prospect").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ID = dr("ProspectID")
        _FName = dr("Firstname") & ""
        _LName = dr("Lastname") & ""
        _SSN = dr("SSN") & ""
        _IsOwner = dr("OwnerFlag")
        _DriverLicense = dr("DriversLicense") & ""
        _MiddleInit = dr("MiddleInit") & ""
        _ProspectNumber = dr("ProspectNumber") & ""
        _CompanyName = dr("CompanyName") & ""
        _Title = dr("Title") & ""
        _Birthdate = dr("BirthDate") & ""
        _SpouseBirthDate = dr("SpouseBirthDate") & ""
        _AnniversaryDate = dr("AnniversaryDate") & ""
        If Not (dr("Income") Is System.DBNull.Value) Then _Income = dr("Income")
        If Not (dr("IncomeDebt") Is System.DBNull.Value) Then _IncomeDebt = dr("IncomeDebt")
        If Not (dr("CreditScore") Is System.DBNull.Value) Then _CreditScore = dr("CreditScore")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _Status = dr("StatusID")
        If Not (dr("Dear") Is System.DBNull.Value) Then _Dear = dr("Dear")
        If Not (dr("MaritalStatusID") Is System.DBNull.Value) Then _MaritalStatusID = dr("MaritalStatusID")
        If Not (dr("SpouseID") Is System.DBNull.Value) Then _SpouseID = dr("SpouseID")
        If Not (dr("SourceID") Is System.DBNull.Value) Then _SourceID = dr("SourceID")
        If Not (dr("ReferringProspectID") Is System.DBNull.Value) Then _ReferringProspectID = dr("ReferringProspectID")
        If Not (dr("DateReferred") Is System.DBNull.Value) Then _DateReferred = dr("DateReferred")
        If Not (dr("AssignedRepID") Is System.DBNull.Value) Then _AssignedRepID = dr("AssignedRepID")
        If Not (dr("FedDNCFlag") Is System.DBNull.Value) Then _FedDNCFlag = CBool(dr("FedDNCFlag"))
        If Not (dr("DriversLicenseStateID") Is System.DBNull.Value) Then _DriverLicenseStateID = dr("DriversLicenseStateID")
        If Not (dr("OccupationID") Is System.DBNull.Value) Then _Occupation = dr("OccupationID")
        If Not (dr("SpouseCreditScore") Is System.DBNull.Value) Then _SpouseCreditScore = dr("SpouseCreditScore")
        If Not (dr("ClubFeeStatusID") Is System.DBNull.Value) Then _ClubFeeStatusID = dr("ClubFeeStatusID")
        _SpouseFirstName = dr("SpouseFirstName") & ""
        _SpouseLastName = dr("SpouseLastName") & ""
        _SpouseSSN = dr("SpouseSSN") & ""
    End Sub

    Public Function Save() As Boolean
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_Prospect where prospectid = " & _ID
        da = New SqlDataAdapter(cm)
        Dim sqlCMBuild As New SqlCommandBuilder(da)
        ds = New DataSet
        da.Fill(ds, "Pros")
        If ds.Tables("Pros").Rows.Count > 0 Then

            dr = ds.Tables("Pros").Rows(0)
        Else
            da.InsertCommand = New SqlCommand("dbo.sp_ProspectInsert", cn)
            da.InsertCommand.CommandType = CommandType.StoredProcedure
            da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.Int, 0, "LocationID")
            da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 50, "FirstName")
            da.InsertCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 50, "LastName")
            da.InsertCommand.Parameters.Add("@MiddleInit", SqlDbType.VarChar, 1, "MiddleInit")
            da.InsertCommand.Parameters.Add("@Dear", SqlDbType.Int, 0, "Dear")
            da.InsertCommand.Parameters.Add("@ProspectNumber", SqlDbType.VarChar, 50, "ProspectNumber")
            da.InsertCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar, 50, "CompanyName")
            da.InsertCommand.Parameters.Add("@Title", SqlDbType.VarChar, 50, "Title")
            da.InsertCommand.Parameters.Add("@BirthDate", SqlDbType.SmallDateTime, 0, "BirthDate")
            da.InsertCommand.Parameters.Add("@MaritalStatusID", SqlDbType.Int, 0, "MaritalStatusID")
            da.InsertCommand.Parameters.Add("@SpouseID", SqlDbType.Int, 0, "SpouseID")
            da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
            da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
            da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
            da.InsertCommand.Parameters.Add("@SourceID", SqlDbType.Int, 0, "SourceID")
            da.InsertCommand.Parameters.Add("@ReferringProspectID", SqlDbType.Int, 0, "ReferringProspectID")
            da.InsertCommand.Parameters.Add("@DateReferred", SqlDbType.SmallDateTime, 0, "DateReferred")
            da.InsertCommand.Parameters.Add("@AssignedRepID", SqlDbType.Int, 0, "AssignedRepID")
            da.InsertCommand.Parameters.Add("@FedDNCFlag", SqlDbType.Bit, 0, "FedDNCFlag")
            da.InsertCommand.Parameters.Add("@SSN", SqlDbType.VarChar, 50, "SSN")
            da.InsertCommand.Parameters.Add("@DriversLicense", SqlDbType.VarChar, 50, "DriversLicense")
            da.InsertCommand.Parameters.Add("@DriversLicenseStateID", SqlDbType.Int, 0, "DriversLicenseStateID")
            da.InsertCommand.Parameters.Add("@Income", SqlDbType.Money, 0, "Income")
            da.InsertCommand.Parameters.Add("@IncomeDebt", SqlDbType.Float, 0, "IncomeDebt")
            da.InsertCommand.Parameters.Add("@CreditScore", SqlDbType.Int, 0, "CreditScore")
            da.InsertCommand.Parameters.Add("@OccupationID", SqlDbType.Int, 0, "OccupationID")
            da.InsertCommand.Parameters.Add("@SpouseFirstName", SqlDbType.VarChar, 50, "SpouseFirstName")
            da.InsertCommand.Parameters.Add("@SpouseLastName", SqlDbType.VarChar, 50, "SpouseLastName")
            da.InsertCommand.Parameters.Add("@SpouseSSN", SqlDbType.VarChar, 50, "SpouseSSN")
            da.InsertCommand.Parameters.Add("@SpouseCreditScore", SqlDbType.Int, 0, "SpouseCreditScore")
            da.InsertCommand.Parameters.Add("@AnniversaryDate", SqlDbType.SmallDateTime, 0, "AnniversaryDate")
            da.InsertCommand.Parameters.Add("@SpouseBirthDate", SqlDbType.DateTime, 0, "SpouseBirthDate")
            da.InsertCommand.Parameters.Add("@ClubFeeStatusID", SqlDbType.Int, 0, "ClubFeeStatusID")
            Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Int, 0, "ProspectID")
            parameter.Direction = ParameterDirection.Output

            dr = ds.Tables("Pros").NewRow
        End If
        Update_Field("LastName", _LName, dr)
        Update_Field("FirstName", _FName, dr)
        Update_Field("MiddleInit", _MiddleInit, dr)
        Update_Field("Dear", _Dear, dr)
        Update_Field("ProspectNumber", _ProspectNumber, dr)
        Update_Field("CompanyName", _CompanyName, dr)
        Update_Field("Title", _Title, dr)
        If _Birthdate <> "" Then
            Update_Field("Birthdate", _Birthdate, dr)
        End If
        'dr("Birthdate") = System.DBNull.Value
        If _SpouseBirthDate <> "" Then
            Update_Field("SpouseBirthDate", _SpouseBirthDate, dr)
        End If
        '_SpouseBirthDate = System.DBNull.Value.ToString


        Update_Field("MaritalStatusID", _MaritalStatusID, dr)
        Update_Field("SpouseID", _SpouseID, dr)
        Update_Field("TypeID", _TypeID, dr)
        Update_Field("SubTypeID", _SubTypeID, dr)
        Update_Field("StatusID", _Status, dr)
        Update_Field("SourceID", _SourceID, dr)
        Update_Field("LocationID", _LocationID, dr)
        Update_Field("ReferringProspectID", _ReferringProspectID, dr)
        If _DateReferred <> "" Then
            Update_Field("DateReferred", _DateReferred, dr)
        End If
        '_DateReferred = System.DBNull.Value.ToString
        If _AnniversaryDate <> "" Then
            Update_Field("AnniversaryDate", _AnniversaryDate, dr)
        End If
        '_AnniversaryDate = System.DBNull.Value.ToString


        Update_Field("AssignedRepID", _AssignedRepID, dr)
        Update_Field("FedDNCFlag", _FedDNCFlag, dr)
        Update_Field("SSN", _SSN, dr)
        Update_Field("DriversLicense", _DriverLicense, dr)
        Update_Field("DriversLicenseStateID", _DriverLicenseStateID, dr)
        Update_Field("Income", _Income, dr)
        Update_Field("IncomeDebt", _IncomeDebt, dr)
        Update_Field("CreditScore", _CreditScore, dr)
        Update_Field("SpouseCreditScore", _SpouseCreditScore, dr)
        Update_Field("OccupationID", _Occupation, dr)
        Update_Field("SpouseFirstName", _SpouseFirstName, dr)
        Update_Field("SpouseLastName", _SpouseLastName, dr)
        Update_Field("SpouseSSN", _SpouseSSN, dr)
        Update_Field("ClubFeeStatusID", _ClubFeeStatusID, dr)
        If ds.Tables("Pros").Rows.Count < 1 Then ds.Tables("Pros").Rows.Add(dr)
        da.Update(ds, "Pros")
        _ID = ds.Tables("Pros").Rows(0).Item("ProspectID")
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return True

        'Catch ex As Exception
        '_Err = ex.ToString
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        'Return False
        'End Try
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
            oEvents.KeyField = "ProspectID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            'If InStr(LCase(sField), "date") > 0 And sValue = "" Then
            '    drow(sField) = System.DBNull.Value
            'Else
            drow(sField) = sValue
            'End If
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub
    Public Function List_For_TourWizard(ByVal phone As String) As SQLDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If phone = "" Then
                ds.SelectCommand = "Select top 100 p.ProspectID, p.LastName, p.FirstName, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & phone & "%'"
            Else
                ds.SelectCommand = "Select top 50 p.ProspectID, p.LastName, p.FirstName, ph.Number as Phone from t_Prospect p left outer join t_ProspectPhone ph on ph.prospectid = p.prospectid where ph.number like '" & phone & "%'"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Owners(ByVal lName As String, ByVal fName As String) As SQLDataSource
        Dim ds As New SQLDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            If fName = "" Then
                ds.SelectCommand = "Select Distinct(p.ProspectID), p.LastName, p.FirstName from t_Contract c inner join t_Prospect p on c.prospectid = p.prospectid where p.Lastname like '" & lName & "%' order by lastname, Firstname"
            Else
                ds.SelectCommand = "Select Distinct(p.ProspectID), p.LastName, p.FirstName from t_Contract c inner join t_Prospect p on c.prospectid = p.prospectid where p.Lastname = '" & lName & "' and p.FirstName like '" & fName & "%' order by lastname, firstname"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Check_Owner(ByVal prosID As Integer) As Boolean
        Dim bOwner As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Contracts from t_Contract where prospectid = " & prosID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("Contracts") = 0 Then
                    bOwner = False
                End If
            Else
                bOwner = False
            End If
            dread.Close()
            If Not (bOwner) Then
                cm.CommandText = "Select Count(*) As Contracts from t_ContractCoOwner where prospectID = " & prosID
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    dread.Read()
                    If dread("Contracts") = 0 Then
                        bOwner = False
                    End If
                Else
                    bOwner = False
                End If
                dread.Close()
            End If
        Catch ex As Exception
            _Err = ex.Message
            bOwner = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bOwner
    End Function

    Public Function val_ProspectID(ByVal prosID As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_prospect where prospectID = " & prosID
            dread = cm.ExecuteReader
            bValid = dread.HasRows
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Protected Overrides Sub Finalize()
        'If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property SpouseCreditScore As Integer
        Get
            Return _SpouseCreditScore
        End Get
        Set(ByVal value As Integer)
            _SpouseCreditScore = value
        End Set
    End Property

    Public Property Occupation() As Integer
        Get
            Return _Occupation
        End Get
        Set(ByVal value As Integer)
            _Occupation = value
        End Set
    End Property

    Public Property Income() As Decimal
        Get
            Return _Income
        End Get
        Set(ByVal value As Decimal)
            _Income = value
        End Set
    End Property

    Public Property IncomeDebt() As Decimal
        Get
            Return _IncomeDebt
        End Get
        Set(ByVal value As Decimal)
            _IncomeDebt = value
        End Set
    End Property

    Public Property CreditScore() As Integer
        Get
            Return _CreditScore
        End Get
        Set(ByVal value As Integer)
            _CreditScore = value
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

    Public Property Error_Message() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

    Public Property Prospect_ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property First_Name() As String
        Get
            Return _FName
        End Get
        Set(ByVal value As String)
            _FName = value
        End Set
    End Property

    Public Property Last_Name() As String
        Get
            Return _LName
        End Get
        Set(ByVal value As String)
            _LName = value
        End Set
    End Property

    Public Property MiddleInit() As String
        Get
            Return _MiddleInit
        End Get
        Set(ByVal value As String)
            _MiddleInit = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property Salutation() As Integer
        Get
            Return _Dear
        End Get
        Set(ByVal value As Integer)
            _Dear = value
        End Set
    End Property

    Public Property ProspectNumber() As String
        Get
            Return _ProspectNumber
        End Get
        Set(ByVal value As String)
            _ProspectNumber = value
        End Set
    End Property

    Public Property CompanyName() As String
        Get
            Return _CompanyName
        End Get
        Set(ByVal value As String)
            _CompanyName = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property

    Public Property BirthDate() As String
        Get
            Return _Birthdate
        End Get
        Set(ByVal value As String)
            _Birthdate = value
        End Set
    End Property

    Public Property SpouseBirthDate() As String
        Get
            Return _SpouseBirthDate
        End Get
        Set(ByVal value As String)
            _SpouseBirthDate = value
        End Set
    End Property

    Public Property DriversLicenseStateID() As Integer
        Get
            Return _DriverLicenseStateID
        End Get
        Set(ByVal value As Integer)
            _DriverLicenseStateID = value
        End Set
    End Property

    Public Property DriversLicense() As String
        Get
            Return _DriverLicense
        End Get
        Set(ByVal value As String)
            _DriverLicense = value
        End Set
    End Property

    Public Property FedDNCFlag() As Boolean
        Get
            Return _FedDNCFlag
        End Get
        Set(ByVal value As Boolean)
            _FedDNCFlag = value
        End Set
    End Property

    Public Property AssignedRepID() As Integer
        Get
            Return _AssignedRepID
        End Get
        Set(ByVal value As Integer)
            _AssignedRepID = value
        End Set
    End Property

    Public Property DateReferred() As String
        Get
            Return _DateReferred
        End Get
        Set(ByVal value As String)
            _DateReferred = value
        End Set
    End Property

    Public Property ReferringProspectID() As Integer
        Get
            Return _ReferringProspectID
        End Get
        Set(ByVal value As Integer)
            _ReferringProspectID = value
        End Set
    End Property

    Public Property SourceID() As Integer
        Get
            Return _SourceID
        End Get
        Set(ByVal value As Integer)
            _SourceID = value
        End Set
    End Property

    Public Property SpouseID() As Integer
        Get
            Return _SpouseID
        End Get
        Set(ByVal value As Integer)
            _SpouseID = value
        End Set
    End Property

    Public Property MaritalStatusID() As Integer
        Get
            Return _MaritalStatusID
        End Get
        Set(ByVal value As Integer)
            _MaritalStatusID = value
        End Set
    End Property

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property SubTypeID() As Integer
        Get
            Return _SubTypeID
        End Get
        Set(ByVal value As Integer)
            _SubTypeID = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _Status
        End Get
        Set(ByVal value As Integer)
            _Status = value
        End Set
    End Property

    Public Property SpouseLastName() As String
        Get
            Return _SpouseLastName
        End Get
        Set(ByVal value As String)
            _SpouseLastName = value
        End Set
    End Property

    Public Property SpouseFirstName() As String
        Get
            Return _SpouseFirstName
        End Get
        Set(ByVal value As String)
            _SpouseFirstName = value
        End Set
    End Property

    Public Property SSN() As String
        Get
            Return _SSN
        End Get
        Set(ByVal value As String)
            _SSN = value
        End Set
    End Property

    Public Property SpouseSSN() As String
        Get
            Return _SpouseSSN
        End Get
        Set(ByVal value As String)
            _SpouseSSN = value
        End Set
    End Property

    Public Property isOwner() As Boolean
        Get
            Return _IsOwner
        End Get
        Set(ByVal value As Boolean)
            _IsOwner = value
        End Set
    End Property

    Public Property AnniversaryDate() As String
        Get
            Return _AnniversaryDate
        End Get
        Set(ByVal value As String)
            _AnniversaryDate = value
        End Set
    End Property

    Public Property ClubFeeStatusID() As Integer
        Get
            Return _ClubFeeStatusID
        End Get
        Set(value As Integer)
            _ClubFeeStatusID = value
        End Set
    End Property
End Class
