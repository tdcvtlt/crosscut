Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Web.UI.WebControls

Public Class clsGolfCourse
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Course As String = ""
    Dim _Address1 As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _PostalCode As String = ""
    Dim _PhoneNumber As String = ""
    Dim _Cost As Decimal = 0
    Dim _NoShowInvoiceID As Integer = 0
    Dim _NoShowInvoiceCost As Decimal = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_GolfCourse where GolfID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_GolfCourse where GolfID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_GolfCourse")
            If ds.Tables("t_GolfCourse").Rows.Count > 0 Then
                dr = ds.Tables("t_GolfCourse").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Course") Is System.DBNull.Value) Then _Course = dr("Course")
        If Not (dr("Address1") Is System.DBNull.Value) Then _Address1 = dr("Address1")
        If Not (dr("City") Is System.DBNull.Value) Then _City = dr("City")
        If Not (dr("StateID") Is System.DBNull.Value) Then _StateID = dr("StateID")
        If Not (dr("PostalCode") Is System.DBNull.Value) Then _PostalCode = dr("PostalCode")
        If Not (dr("PhoneNumber") Is System.DBNull.Value) Then _PhoneNumber = dr("PhoneNumber")
        If Not (dr("Cost") Is System.DBNull.Value) Then _Cost = dr("Cost")
        If Not (dr("NoShowInvoiceID") Is System.DBNull.Value) Then _NoShowInvoiceID = dr("NoShowInvoiceID")
        If Not (dr("NoShowInvoiceCost") Is System.DBNull.Value) Then _NoShowInvoiceCost = dr("NoShowInvoiceCost")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_GolfCourse where GolfID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_GolfCourse")
            If ds.Tables("t_GolfCourse").Rows.Count > 0 Then
                dr = ds.Tables("t_GolfCourse").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_GolfCourseInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Course", SqlDbType.VarChar, 0, "Course")
                da.InsertCommand.Parameters.Add("@Address1", SqlDbType.VarChar, 0, "Address1")
                da.InsertCommand.Parameters.Add("@City", SqlDbType.VarChar, 0, "City")
                da.InsertCommand.Parameters.Add("@StateID", SqlDbType.Int, 0, "StateID")
                da.InsertCommand.Parameters.Add("@PostalCode", SqlDbType.VarChar, 0, "PostalCode")
                da.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 0, "PhoneNumber")
                da.InsertCommand.Parameters.Add("@Cost", SqlDbType.Money, 0, "Cost")
                da.InsertCommand.Parameters.Add("@NoShowInvoiceID", SqlDbType.Int, 0, "NoShowInvoiceID")
                da.InsertCommand.Parameters.Add("@NoShowInvoiceCost", SqlDbType.Money, 0, "NoShowInvoiceCost")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@GolfID", SqlDbType.Int, 0, "GolfID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_GolfCourse").NewRow
            End If
            Update_Field("Course", _Course, dr)
            Update_Field("Address1", _Address1, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("PhoneNumber", _PhoneNumber, dr)
            Update_Field("Cost", _Cost, dr)
            Update_Field("NoShowInvoiceID", _NoShowInvoiceID, dr)
            Update_Field("NoShowInvoiceCost", _NoShowInvoiceCost, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_GolfCourse").Rows.Count < 1 Then ds.Tables("t_GolfCourse").Rows.Add(dr)
            da.Update(ds, "t_GolfCourse")
            _ID = ds.Tables("t_GolfCourse").Rows(0).Item("GolfID")
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
            oEvents.KeyField = "GolfID"
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

    Public Function List_Courses(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If filter = "" Then
            ds.SelectCommand = "Select GolfID as ID, Course from t_GolfCourse order by Course asc"
        Else
            ds.SelectCommand = "Select GolfID as ID, Course from t_GolfCourse where Course Like '" & filter & "%' order by Course asc"
        End If
        Return ds
    End Function

    Public Function List_Active_Courses(ByVal ID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select GolfID as ID, Course from t_GolfCourse where active = 1 or GolfID in (Select GolfID from t_res2Golf where res2golfid = '" & ID & "') order by Course asc"
        Return ds
    End Function
    Public Property Course() As String
        Get
            Return _Course
        End Get
        Set(ByVal value As String)
            _Course = value
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

    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Public Property StateID() As Integer
        Get
            Return _StateID
        End Get
        Set(ByVal value As Integer)
            _StateID = value
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

    Public Property Cost() As Decimal
        Get
            Return _Cost
        End Get
        Set(ByVal value As Decimal)
            _Cost = value
        End Set
    End Property

    Public Property NoShowInvoiceID() As Integer
        Get
            Return _NoShowInvoiceID
        End Get
        Set(ByVal value As Integer)
            _NoShowInvoiceID = value
        End Set
    End Property

    Public Property NoShowInvoiceCost() As Decimal
        Get
            Return _NoShowInvoiceCost
        End Get
        Set(ByVal value As Decimal)
            _NoShowInvoiceCost = value
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

    Public Property GolfID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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
End Class
