Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsChecks
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _CheckNo As Integer = 0
    Dim _PosOnPage As Integer = 0
    Dim _Printed As Boolean = False
    Dim _DatePrinted As String = ""
    Dim _Voided As Boolean = False
    Dim _DateVoided As String = ""
    Dim _PrintedByID As Integer = 0
    Dim _VoidedByID As Integer = 0
    Dim _Account As String = ""
    Dim _AccountID As Integer = 0
    Dim _PremiumIssuedID As Integer = 0
    Dim _TourID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Checks where CheckID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Checks where CheckID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Checks")
            If ds.Tables("t_Checks").Rows.Count > 0 Then
                dr = ds.Tables("t_Checks").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("CheckNo") Is System.DBNull.Value) Then _CheckNo = dr("CheckNo")
        If Not (dr("PosOnPage") Is System.DBNull.Value) Then _PosOnPage = dr("PosOnPage")
        If Not (dr("Printed") Is System.DBNull.Value) Then _Printed = dr("Printed")
        If Not (dr("DatePrinted") Is System.DBNull.Value) Then _DatePrinted = dr("DatePrinted")
        If Not (dr("Voided") Is System.DBNull.Value) Then _Voided = dr("Voided")
        If Not (dr("DateVoided") Is System.DBNull.Value) Then _DateVoided = dr("DateVoided")
        If Not (dr("PrintedByID") Is System.DBNull.Value) Then _PrintedByID = dr("PrintedByID")
        If Not (dr("VoidedByID") Is System.DBNull.Value) Then _VoidedByID = dr("VoidedByID")
        If Not (dr("Account") Is System.DBNull.Value) Then _Account = dr("Account")
        If Not (dr("AccountID") Is System.DBNull.Value) Then _AccountID = dr("AccountID")
        If Not (dr("PremiumIssuedID") Is System.DBNull.Value) Then _PremiumIssuedID = dr("PremiumIssuedID")
        If Not (dr("TourID") Is System.DBNull.Value) Then _TourID = dr("TourID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Checks where CheckID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Checks")
            If ds.Tables("t_Checks").Rows.Count > 0 Then
                dr = ds.Tables("t_Checks").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ChecksInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@CheckNo", SqlDbType.int, 0, "CheckNo")
                da.InsertCommand.Parameters.Add("@PosOnPage", SqlDbType.int, 0, "PosOnPage")
                da.InsertCommand.Parameters.Add("@Printed", SqlDbType.bit, 0, "Printed")
                da.InsertCommand.Parameters.Add("@DatePrinted", SqlDbType.datetime, 0, "DatePrinted")
                da.InsertCommand.Parameters.Add("@Voided", SqlDbType.bit, 0, "Voided")
                da.InsertCommand.Parameters.Add("@DateVoided", SqlDbType.datetime, 0, "DateVoided")
                da.InsertCommand.Parameters.Add("@PrintedByID", SqlDbType.int, 0, "PrintedByID")
                da.InsertCommand.Parameters.Add("@VoidedByID", SqlDbType.int, 0, "VoidedByID")
                da.InsertCommand.Parameters.Add("@Account", SqlDbType.varchar, 0, "Account")
                da.InsertCommand.Parameters.Add("@AccountID", SqlDbType.int, 0, "AccountID")
                da.InsertCommand.Parameters.Add("@PremiumIssuedID", SqlDbType.int, 0, "PremiumIssuedID")
                da.InsertCommand.Parameters.Add("@TourID", SqlDbType.int, 0, "TourID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CheckID", SqlDbType.Int, 0, "CheckID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Checks").NewRow
            End If
            Update_Field("CheckNo", _CheckNo, dr)
            Update_Field("PosOnPage", _PosOnPage, dr)
            Update_Field("Printed", _Printed, dr)
            Update_Field("DatePrinted", _DatePrinted, dr)
            Update_Field("Voided", _Voided, dr)
            Update_Field("DateVoided", _DateVoided, dr)
            Update_Field("PrintedByID", _PrintedByID, dr)
            Update_Field("VoidedByID", _VoidedByID, dr)
            Update_Field("Account", _Account, dr)
            Update_Field("AccountID", _AccountID, dr)
            Update_Field("PremiumIssuedID", _PremiumIssuedID, dr)
            Update_Field("TourID", _TourID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Checks").Rows.Count < 1 Then ds.Tables("t_Checks").Rows.Add(dr)
            da.Update(ds, "t_Checks")
            _ID = ds.Tables("t_Checks").Rows(0).Item("CheckID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "CheckID"
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

    Public Function Check_Lookup(ByVal checkNum As String) As SQLDataSource
        Dim ds As New SQLDataSOurce
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select * from v_ChecksReport where CheckNo = '" & checkNum & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Add_Check(ByVal checkNo As Integer, ByVal pagePos As Integer, ByVal loc As Integer) As Boolean
        Dim bAdded As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Insert Into t_Checks(CheckNo, PosOnPage, LocationID) Values (" & checkNo & "," & pagePos & "," & loc & ")"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bAdded = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bAdded
    End Function
    Public Function Validate_Check_Range(ByVal start As Integer, ByVal endNum As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end as checks from t_Checks where checkNo >= " & start & " and checkno <= " & endNum & ""
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Checks") > 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Check_Report(ByVal sDate As Date, ByVal endDate As Date, ByVal locID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select * from v_ChecksReport where Location in (select comboitem from t_Comboitems where comboitemid = '" & locID & "') and (DatePrinted between '" & sDate & "' and '" & endDate.AddDays(1) & "' or DateVoided between '" & sDate & "' and '" & endDate.AddDays(1) & "')"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Validate_Check_Void(ByVal checkNum As String) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when Count(*) is null then 0 else Count(*) end As Checks from t_Checks where checkNo = '" & checkNum & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Checks") = 0 Then
                bValid = False
            End If
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Void_Check(ByVal checkNum As String) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_Checks set Voided = 1, DateVoided = '" & System.DateTime.Now & "', VoidedByID = '" & _UserID & "' where checkNo = '" & checkNum & "'"
            cm.ExecuteNonQuery()
            cm.CommandText = "Update t_Premium set QtyOnHand = QtyOnHand + 1 where premiumname = 'Check'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Get_Printable_Checks(ByVal locID As Integer) As SQLDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select PremiumIssuedID, LastName + ', ' + FirstName as Name, TourID, Amount, '' as CheckNumber from v_ChecksToPrint where TourLocationID = '" & locID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Get_Available_Checks(ByVal sNum As Integer) As String()
        Dim checks(0) As String
        Dim i As Integer = 0
        checks(0) = "Err"
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select CheckNo from t_Checks where premiumissuedid = 0 and Voided = 0 and CheckNo >= " & sNum & " order by checkNo"
            dread = cm.ExecuteReader
            Do While dread.Read
                ReDim Preserve checks(i)
                checks(i) = dread("CheckNo")
                i = i + 1
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return checks
    End Function
    Public Property CheckNo() As Integer
        Get
            Return _CheckNo
        End Get
        Set(ByVal value As Integer)
            _CheckNo = value
        End Set
    End Property

    Public Property PosOnPage() As Integer
        Get
            Return _PosOnPage
        End Get
        Set(ByVal value As Integer)
            _PosOnPage = value
        End Set
    End Property

    Public Property Printed() As Boolean
        Get
            Return _Printed
        End Get
        Set(ByVal value As Boolean)
            _Printed = value
        End Set
    End Property

    Public Property DatePrinted() As String
        Get
            Return _DatePrinted
        End Get
        Set(ByVal value As String)
            _DatePrinted = value
        End Set
    End Property

    Public Property Voided() As Boolean
        Get
            Return _Voided
        End Get
        Set(ByVal value As Boolean)
            _Voided = value
        End Set
    End Property

    Public Property DateVoided() As String
        Get
            Return _DateVoided
        End Get
        Set(ByVal value As String)
            _DateVoided = value
        End Set
    End Property

    Public Property PrintedByID() As Integer
        Get
            Return _PrintedByID
        End Get
        Set(ByVal value As Integer)
            _PrintedByID = value
        End Set
    End Property

    Public Property VoidedByID() As Integer
        Get
            Return _VoidedByID
        End Get
        Set(ByVal value As Integer)
            _VoidedByID = value
        End Set
    End Property

    Public Property Account() As String
        Get
            Return _Account
        End Get
        Set(ByVal value As String)
            _Account = value
        End Set
    End Property

    Public Property AccountID() As Integer
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Integer)
            _AccountID = value
        End Set
    End Property

    Public Property PremiumIssuedID() As Integer
        Get
            Return _PremiumIssuedID
        End Get
        Set(ByVal value As Integer)
            _PremiumIssuedID = value
        End Set
    End Property

    Public Property TourID() As Integer
        Get
            Return _TourID
        End Get
        Set(ByVal value As Integer)
            _TourID = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
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
    Public ReadOnly Property CheckID() As Integer
        Get
            Return _ID
        End Get
    End Property
End Class
