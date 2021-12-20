Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadProgram
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PictureInterval As Integer = 0
    Dim _UsePictures As Boolean = False
    Dim _url As String = ""
    Dim _exeScript As String = ""
    Dim _exetimer As Integer = 0
    Dim _screensaver As String = ""
    Dim _screentimer As Integer = 0
    Dim _termstimer As Integer = 0
    Dim _registration As Integer = 0
    Dim _Description As String = ""
    Dim _EntryFormID As Integer = 0
    Dim _VendorID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadProgram where LeadProgramID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadProgram where LeadProgramID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram")
            If ds.Tables("t_LeadProgram").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PictureInterval") Is System.DBNull.Value) Then _PictureInterval = dr("PictureInterval")
        If Not (dr("UsePictures") Is System.DBNull.Value) Then _UsePictures = dr("UsePictures")
        If Not (dr("url") Is System.DBNull.Value) Then _url = dr("url")
        If Not (dr("exeScript") Is System.DBNull.Value) Then _exeScript = dr("exeScript")
        If Not (dr("exetimer") Is System.DBNull.Value) Then _exetimer = dr("exetimer")
        If Not (dr("screensaver") Is System.DBNull.Value) Then _screensaver = dr("screensaver")
        If Not (dr("screentimer") Is System.DBNull.Value) Then _screentimer = dr("screentimer")
        If Not (dr("termstimer") Is System.DBNull.Value) Then _termstimer = dr("termstimer")
        If Not (dr("registration") Is System.DBNull.Value) Then _registration = dr("registration")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("EntryFormID") Is System.DBNull.Value) Then _EntryFormID = dr("EntryFormID")
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadProgram where LeadProgramID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram")
            If ds.Tables("t_LeadProgram").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadProgramInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PictureInterval", SqlDbType.int, 0, "PictureInterval")
                da.InsertCommand.Parameters.Add("@UsePictures", SqlDbType.bit, 0, "UsePictures")
                da.InsertCommand.Parameters.Add("@url", SqlDbType.varchar, 0, "url")
                da.InsertCommand.Parameters.Add("@exeScript", SqlDbType.varchar, 0, "exeScript")
                da.InsertCommand.Parameters.Add("@exetimer", SqlDbType.int, 0, "exetimer")
                da.InsertCommand.Parameters.Add("@screensaver", SqlDbType.varchar, 0, "screensaver")
                da.InsertCommand.Parameters.Add("@screentimer", SqlDbType.int, 0, "screentimer")
                da.InsertCommand.Parameters.Add("@termstimer", SqlDbType.int, 0, "termstimer")
                da.InsertCommand.Parameters.Add("@registration", SqlDbType.int, 0, "registration")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@EntryFormID", SqlDbType.Int, 0, "EntryFormID")
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.Int, 0, "VendorID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LeadProgramID", SqlDbType.Int, 0, "LeadProgramID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadProgram").NewRow
            End If
            Update_Field("PictureInterval", _PictureInterval, dr)
            Update_Field("UsePictures", _UsePictures, dr)
            Update_Field("url", _url, dr)
            Update_Field("exeScript", _exeScript, dr)
            Update_Field("exetimer", _exetimer, dr)
            Update_Field("screensaver", _screensaver, dr)
            Update_Field("screentimer", _screentimer, dr)
            Update_Field("termstimer", _termstimer, dr)
            Update_Field("registration", _registration, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("EntryFormID", _EntryFormID, dr)
            Update_Field("VendorID", _VendorID, dr)
            If ds.Tables("t_LeadProgram").Rows.Count < 1 Then ds.Tables("t_LeadProgram").Rows.Add(dr)
            da.Update(ds, "t_LeadProgram")
            _ID = ds.Tables("t_LeadProgram").Rows(0).Item("LeadProgramID")
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
            oEvents.KeyField = "LeadProgramID"
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


    Public Function Update_All_Versions(ByVal leadProgramID As Integer, ByVal increment As Double) As Boolean
        Dim bUpdate As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_LeadProgram2Location set Version = Version + " & increment & " where LeadProgramID = " & leadProgramID
            cm.ExecuteNonQuery()
            bUpdate = True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bUpdate
    End Function

    Public Function Update_Version(ByVal leadProgramID As Integer, ByVal locationID As Integer, ByVal increment As Double) As Boolean
        Dim bUpdate As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_LeadProgram2Location set Version = Version + " & increment & " where LeadProgramID = " & leadProgramID & " and LocationID = " & locationID
            cm.ExecuteNonQuery()
            bUpdate = True
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bUpdate
    End Function

    Public Function List_Programs() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select LeadProgramID as ID, Description from t_LeadProgram order by LeadProgramID"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Active_Location(ByVal oem As String, ByVal sDate As Date) As Integer
        Dim locID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select LocationID from t_LeadProgram2Location ll inner join t_LeadProgram2Device ld on ll.LeadProgramID = ld.LeadProgramID where ld.Device = '" & oem & "' and ld.Active = 1 and ll.StartDate <= '" & sDate & "' and ll.EndDate >= '" & sDate & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                locID = dread("LocationID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return locID
    End Function

    Public Function Get_Vendor(ByVal oem As String) As Integer
        Dim vendorID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select lp.VendorID from t_LeadProgram lp inner join t_LeadProgram2Device ld on lp.LeadProgramID = ld.LeadProgramID where ld.Device = '" & oem & "' and ld.Active = 1"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                vendorID = dread("VendorID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return vendorID
    End Function

    Public Property PictureInterval() As Integer
        Get
            Return _PictureInterval
        End Get
        Set(ByVal value As Integer)
            _PictureInterval = value
        End Set
    End Property

    Public Property UsePictures() As Boolean
        Get
            Return _UsePictures
        End Get
        Set(ByVal value As Boolean)
            _UsePictures = value
        End Set
    End Property

    Public Property url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property

    Public Property exeScript() As String
        Get
            Return _exeScript
        End Get
        Set(ByVal value As String)
            _exeScript = value
        End Set
    End Property

    Public Property exetimer() As Integer
        Get
            Return _exetimer
        End Get
        Set(ByVal value As Integer)
            _exetimer = value
        End Set
    End Property

    Public Property screensaver() As String
        Get
            Return _screensaver
        End Get
        Set(ByVal value As String)
            _screensaver = value
        End Set
    End Property

    Public Property screentimer() As Integer
        Get
            Return _screentimer
        End Get
        Set(ByVal value As Integer)
            _screentimer = value
        End Set
    End Property

    Public Property termstimer() As Integer
        Get
            Return _termstimer
        End Get
        Set(ByVal value As Integer)
            _termstimer = value
        End Set
    End Property

    Public Property registration() As Integer
        Get
            Return _registration
        End Get
        Set(ByVal value As Integer)
            _registration = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property EntryFormID() As Integer
        Get
            Return _EntryFormID
        End Get
        Set(ByVal value As Integer)
            _EntryFormID = value
        End Set
    End Property

    Public Property VendorID() As Integer
        Get
            Return _VendorID
        End Get
        Set(value As Integer)
            _VendorID = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

    Public Property LeadProgramID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
