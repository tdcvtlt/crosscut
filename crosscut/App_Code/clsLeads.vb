Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeads
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DrawingID As Integer = 0
    Dim _Source As String = ""
    Dim _KioskID As Integer = 0
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _SpouseName As String = ""
    Dim _Address1 As String = ""
    Dim _Address2 As String = ""
    Dim _City As String = ""
    Dim _State As String = ""
    Dim _PostalCode As String = ""
    Dim _PhoneNumber As String = ""
    Dim _EmailAddress As String = ""
    Dim _Age As String = ""
    Dim _MaritalStatus As String = ""
    Dim _IncomeRange As String = ""
    Dim _OwnRent As String = ""
    Dim _Signed As Boolean = False
    Dim _SignDate As String = ""
    Dim _LeadFileID As Integer = 0
    Dim _Qualified As Boolean = False
    Dim _DNC As Boolean = False
    Dim _DateEntered As String = ""
    Dim _ProspectID As Integer = 0
    Dim _ReferringLeadID As Integer = 0
    Dim _DateCollected As String = ""
    Dim _CallType As String = ""
    Dim _MaleFemale As String = ""
    Dim _VendorID As Integer = 0
    Dim _EntryForm As Integer = 0
    Dim _ScanFileID As Integer = 0
    Dim _Gifts As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Leads where LeadID = " & _ID, cn)
    End Sub

    Public Function List(Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "") As SqlDataSource
        If sFilterField <> "" Then
            Return New SqlDataSource(Resources.Resource.cns, "Select * from t_Leads where " & sFilterField & " like '" & sFilterValue & "%' order by " & sFilterField)
        Else
            Return New SqlDataSource(Resources.Resource.cns, "Select * from t_Leads order by lastname, firstname")
        End If
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Leads where LeadID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Leads")
            If ds.Tables("t_Leads").Rows.Count > 0 Then
                dr = ds.Tables("t_Leads").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DrawingID") Is System.DBNull.Value) Then _DrawingID = dr("DrawingID")
        If Not (dr("Source") Is System.DBNull.Value) Then _Source = dr("Source")
        If Not (dr("KioskID") Is System.DBNull.Value) Then _KioskID = dr("KioskID")
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("SpouseName") Is System.DBNull.Value) Then _SpouseName = dr("SpouseName")
        If Not (dr("Address1") Is System.DBNull.Value) Then _Address1 = dr("Address1")
        If Not (dr("Address2") Is System.DBNull.Value) Then _Address2 = dr("Address2")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("State") Is System.DBNull.Value) Then _State = dr("State")
        If Not (dr("PostalCode") Is System.DBNull.Value) Then _PostalCode = dr("PostalCode")
        If Not (dr("PhoneNumber") Is System.DBNull.Value) Then _PhoneNumber = dr("PhoneNumber")
        If Not (dr("EmailAddress") Is System.DBNull.Value) Then _EmailAddress = dr("EmailAddress")
        If Not (dr("Age") Is System.DBNull.Value) Then _Age = dr("Age")
        If Not (dr("MaritalStatus") Is System.DBNull.Value) Then _MaritalStatus = dr("MaritalStatus")
        If Not (dr("IncomeRange") Is System.DBNull.Value) Then _IncomeRange = dr("IncomeRange")
        If Not (dr("OwnRent") Is System.DBNull.Value) Then _OwnRent = dr("OwnRent")
        If Not (dr("Signed") Is System.DBNull.Value) Then _Signed = dr("Signed")
        If Not (dr("SignDate") Is System.DBNull.Value) Then _SignDate = dr("SignDate")
        If Not (dr("LeadFileID") Is System.DBNull.Value) Then _LeadFileID = dr("LeadFileID")
        If Not (dr("Qualified") Is System.DBNull.Value) Then _Qualified = dr("Qualified")
        If Not (dr("DNC") Is System.DBNull.Value) Then _DNC = dr("DNC")
        If Not (dr("DateEntered") Is System.DBNull.Value) Then _DateEntered = dr("DateEntered")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("ReferringLeadID") Is System.DBNull.Value) Then _ReferringLeadID = dr("ReferringLeadID")
        If Not (dr("DateCollected") Is System.DBNull.Value) Then _DateCollected = dr("DateCollected")
        If Not (dr("CallType") Is System.DBNull.Value) Then _CallType = dr("CallType")
        If Not (dr("Male/Female") Is System.DBNull.Value) Then _MaleFemale = dr("Male/Female")
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
        If Not (dr("EntryForm") Is System.DBNull.Value) Then _EntryForm = dr("EntryForm")
        If Not (dr("ScanFileID") Is System.DBNull.Value) Then _ScanFileID = dr("ScanFileID")
        If Not (dr("Gifts") Is System.DBNull.Value) Then _Gifts = dr("Gifts")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Leads where LeadID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Leads")
            If ds.Tables("t_Leads").Rows.Count > 0 Then
                dr = ds.Tables("t_Leads").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DrawingID", SqlDbType.Int, 0, "DrawingID")
                da.InsertCommand.Parameters.Add("@Source", SqlDbType.VarChar, 0, "Source")
                da.InsertCommand.Parameters.Add("@KioskID", SqlDbType.Int, 0, "KioskID")
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@SpouseName", SqlDbType.VarChar, 0, "SpouseName")
                da.InsertCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 0, "Address1")
                da.InsertCommand.Parameters.Add("@Address2", SqlDbType.VarChar, 0, "Address2")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 0, "City")
                da.InsertCommand.Parameters.Add("@State", SqlDbType.VarChar, 0, "State")
                da.InsertCommand.Parameters.Add("@PostalCode", SqlDbType.VarChar, 0, "PostalCode")
                da.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 0, "PhoneNumber")
                da.InsertCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 0, "EmailAddress")
                da.InsertCommand.Parameters.Add("@Age", SqlDbType.VarChar, 0, "Age")
                da.InsertCommand.Parameters.Add("@MaritalStatus", SqlDbType.VarChar, 0, "MaritalStatus")
                da.InsertCommand.Parameters.Add("@IncomeRange", SqlDbType.VarChar, 0, "IncomeRange")
                da.InsertCommand.Parameters.Add("@OwnRent", SqlDbType.VarChar, 0, "OwnRent")
                da.InsertCommand.Parameters.Add("@Signed", SqlDbType.Bit, 0, "Signed")
                da.InsertCommand.Parameters.Add("@SignDate", SqlDbType.DateTime, 0, "SignDate")
                da.InsertCommand.Parameters.Add("@LeadFileID", SqlDbType.Int, 0, "LeadFileID")
                da.InsertCommand.Parameters.Add("@Qualified", SqlDbType.Bit, 0, "Qualified")
                da.InsertCommand.Parameters.Add("@DNC", SqlDbType.Bit, 0, "DNC")
                da.InsertCommand.Parameters.Add("@DateEntered", SqlDbType.SmallDateTime, 0, "DateEntered")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@ReferringLeadID", SqlDbType.Int, 0, "ReferringLeadID")
                da.InsertCommand.Parameters.Add("@DateCollected", SqlDbType.VarChar, 0, "DateCollected")
                da.InsertCommand.Parameters.Add("@MF", SqlDbType.VarChar, 0, "Male/Female")
                da.InsertCommand.Parameters.Add("@CallType", SqlDbType.VarChar, 0, "CallType")
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.Int, 0, "VendorID")
                da.InsertCommand.Parameters.Add("@EntryForm", SqlDbType.Int, 0, "EntryForm")
                da.InsertCommand.Parameters.Add("@ScanFileID", SqlDbType.Int, 0, "ScanFileID")
                da.InsertCommand.Parameters.Add("@Gifts", SqlDbType.VarChar, 0, "Gifts")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LeadID", SqlDbType.Int, 0, "LeadID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Leads").NewRow
            End If
            Update_Field("DrawingID", _DrawingID, dr)
            Update_Field("Source", _Source, dr)
            Update_Field("KioskID", _KioskID, dr)
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("SpouseName", _SpouseName, dr)
            Update_Field("Address1", _Address1, dr)
            Update_Field("Address2", _Address2, dr)
            Update_Field("City", _City, dr)
            Update_Field("State", _State, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("PhoneNumber", _PhoneNumber, dr)
            Update_Field("EmailAddress", _EmailAddress, dr)
            Update_Field("Age", _Age, dr)
            Update_Field("MaritalStatus", _MaritalStatus, dr)
            Update_Field("IncomeRange", _IncomeRange, dr)
            Update_Field("OwnRent", _OwnRent, dr)
            Update_Field("Signed", _Signed, dr)
            Update_Field("SignDate", _SignDate, dr)
            Update_Field("LeadFileID", _LeadFileID, dr)
            Update_Field("Qualified", _Qualified, dr)
            Update_Field("DNC", _DNC, dr)
            Update_Field("DateEntered", _DateEntered, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ReferringLeadID", _ReferringLeadID, dr)
            Update_Field("DateCollected", _DateCollected, dr)
            Update_Field("CallType", _CallType, dr)
            Update_Field("Male/Female", _MaleFemale, dr)
            Update_Field("VendorID", _VendorID, dr)
            Update_Field("EntryForm", _EntryForm, dr)
            Update_Field("ScanFileID", _ScanFileID, dr)
            Update_Field("Gifts", _Gifts, dr)
            If ds.Tables("t_Leads").Rows.Count < 1 Then ds.Tables("t_Leads").Rows.Add(dr)
            da.Update(ds, "t_Leads")
            _ID = ds.Tables("t_Leads").Rows(0).Item("LeadID")
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
            oEvents.KeyField = "LeadID"
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

    Public Function Get_Area_Codes() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct(Left(Replace(PhoneNumber, ' ',''), 3)) as AreaCode from t_Leads where Len(PhoneNumber) = 10 order by Left(Replace(phonenumber, ' ', ''), 3) asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Check_Duplicate(ByVal phone As String, ByVal email As String, ByVal sdate As Date) As Boolean
        Dim bFound As Boolean = False
        Try
            Dim oDraw As New clsLeadDrawing
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Leads from t_leads where phoneNumber = '" & phone & "' and case when emailaddress is null then '' else emailaddress end = '" & email & "' and DrawingID = " & oDraw.Get_Active_Drawing(sdate)
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Leads") > 0 Then
                bFound = True
            End If
            dread.Close()
            oDraw = Nothing
        Catch ex As Exception
            _Err = ex.Message
            bFound = True
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bFound
    End Function
    Public Property DrawingID() As Integer
        Get
            Return _DrawingID
        End Get
        Set(ByVal value As Integer)
            _DrawingID = value
        End Set
    End Property

    Public Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property

    Public Property KioskID() As Integer
        Get
            Return _KioskID
        End Get
        Set(ByVal value As Integer)
            _KioskID = value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    Public Property SpouseName() As String
        Get
            Return _SpouseName
        End Get
        Set(ByVal value As String)
            _SpouseName = value
        End Set
    End Property

    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
        End Set
    End Property

    Public Property PostalCode() As String
        Get
            Return _PostalCode
        End Get
        Set(ByVal value As String)
            _PostalCode = value
        End Set
    End Property

    Public Property PhoneNumber() As String
        Get
            Return _PhoneNumber
        End Get
        Set(ByVal value As String)
            _PhoneNumber = value
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

    Public Property Age() As String
        Get
            Return _Age
        End Get
        Set(ByVal value As String)
            _Age = value
        End Set
    End Property

    Public Property MaritalStatus() As String
        Get
            Return _MaritalStatus
        End Get
        Set(ByVal value As String)
            _MaritalStatus = value
        End Set
    End Property

    Public Property IncomeRange() As String
        Get
            Return _IncomeRange
        End Get
        Set(ByVal value As String)
            _IncomeRange = value
        End Set
    End Property

    Public Property OwnRent() As String
        Get
            Return _OwnRent
        End Get
        Set(ByVal value As String)
            _OwnRent = value
        End Set
    End Property

    Public Property Signed() As Boolean
        Get
            Return _Signed
        End Get
        Set(ByVal value As Boolean)
            _Signed = value
        End Set
    End Property

    Public Property SignDate() As String
        Get
            Return _SignDate
        End Get
        Set(ByVal value As String)
            _SignDate = value
        End Set
    End Property

    Public Property LeadFileID() As Integer
        Get
            Return _LeadFileID
        End Get
        Set(ByVal value As Integer)
            _LeadFileID = value
        End Set
    End Property

    Public Property Qualified() As Boolean
        Get
            Return _Qualified
        End Get
        Set(ByVal value As Boolean)
            _Qualified = value
        End Set
    End Property

    Public Property DNC() As Boolean
        Get
            Return _DNC
        End Get
        Set(ByVal value As Boolean)
            _DNC = value
        End Set
    End Property

    Public Property DateEntered As String
        Get
            Return _DateEntered
        End Get
        Set(ByVal value As String)
            _DateEntered = value
        End Set
    End Property

    Public Property LeadID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property ReferringLeadID() As Integer
        Get
            Return _ReferringLeadID
        End Get
        Set(value As Integer)
            _ReferringLeadID = value
        End Set
    End Property

    Public Property MaleFemale As String
        Get
            Return _MaleFemale
        End Get
        Set(ByVal value As String)
            _MaleFemale = value
        End Set
    End Property

    Public Property DateCollected As String
        Get
            Return _DateCollected
        End Get
        Set(value As String)
            _DateCollected = value
        End Set
    End Property

    Public Property CallType As String
        Get
            Return _CallType
        End Get
        Set(value As String)
            _CallType = value
        End Set
    End Property

    Public Property VendorID() As Integer
        Get
            Return _VendorID
        End Get
        Set(ByVal value As Integer)
            _VendorID = value
        End Set
    End Property

    Public Property EntryForm() As Integer
        Get
            Return _EntryForm
        End Get
        Set(ByVal value As Integer)
            _EntryForm = value
        End Set
    End Property

    Public Property ScanFileID() As Integer
        Get
            Return _ScanFileID
        End Get
        Set(ByVal value As Integer)
            _ScanFileID = value
        End Set
    End Property

    Public Property Err As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

    Public Property Gifts As String
        Get
            Return _Gifts
        End Get
        Set(value As String)
            _Gifts = value
        End Set
    End Property
End Class
