Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPALRequest
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _ManagerApproved As Integer = 0
    Dim _ManagerID As Integer = 0
    Dim _DateApproved As String = ""
    Dim _TotalPALHours As Decimal = 0
    Dim _TotalSSLBHours As Decimal = 0
    Dim _TotalUnpaidHours As Decimal = 0
    Dim _Scheduled As Boolean = False
    Dim _DoctorsNote As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PALRequest where PALRequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PALRequest where PALRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PALRequest")
            If ds.Tables("t_PALRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_PALRequest").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("ManagerApproved") Is System.DBNull.Value) Then _ManagerApproved = dr("ManagerApproved")
        If Not (dr("ManagerID") Is System.DBNull.Value) Then _ManagerID = dr("ManagerID")
        If Not (dr("DateApproved") Is System.DBNull.Value) Then _DateApproved = dr("DateApproved")
        If Not (dr("TotalPALHours") Is System.DBNull.Value) Then _TotalPALHours = dr("TotalPALHours")
        If Not (dr("TotalSSLBHours") Is System.DBNull.Value) Then _TotalSSLBHours = dr("TotalSSLBHours")
        If Not (dr("TotalUnpaidHours") Is System.DBNull.Value) Then _TotalUnpaidHours = dr("TotalUnpaidHours")
        If Not (dr("Scheduled") Is System.DBNull.Value) Then _Scheduled = dr("Scheduled")
        If Not (dr("DoctorsNote") Is System.DBNull.Value) Then _DoctorsNote = dr("DoctorsNote")

    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PALRequest where PALRequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PALRequest")
            If ds.Tables("t_PALRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_PALRequest").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PALRequestInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.Int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.Int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@ManagerApproved", SqlDbType.Int, 0, "ManagerApproved")
                da.InsertCommand.Parameters.Add("@ManagerID", SqlDbType.Int, 0, "ManagerID")
                da.InsertCommand.Parameters.Add("@DateApproved", SqlDbType.DateTime, 0, "DateApproved")
                da.InsertCommand.Parameters.Add("@TotalPALHours", SqlDbType.Money, 0, "TotalPALHours")
                da.InsertCommand.Parameters.Add("@TotalSSLBHours", SqlDbType.Money, 0, "TotalSSLBHours")
                da.InsertCommand.Parameters.Add("@TotalUnpaidHours", SqlDbType.Money, 0, "TotalUnpaidHours")
                da.InsertCommand.Parameters.Add("@Scheduled", SqlDbType.Bit, 0, "Scheduled")
                da.InsertCommand.Parameters.Add("@DoctorsNote", SqlDbType.Money, 0, "DoctorsNote")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PALRequestID", SqlDbType.Int, 0, "PALRequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PALRequest").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("ManagerApproved", _ManagerApproved, dr)
            Update_Field("ManagerID", _ManagerID, dr)
            Update_Field("DateApproved", _DateApproved, dr)
            Update_Field("TotalPALHours", _TotalPALHours, dr)
            Update_Field("TotalSSLBHours", _TotalSSLBHours, dr)
            Update_Field("TotalUnpaidHours", _TotalUnpaidHours, dr)
            Update_Field("Scheduled", _Scheduled, dr)
            Update_Field("DoctorsNote", _DoctorsNote, dr)
            If ds.Tables("t_PALRequest").Rows.Count < 1 Then ds.Tables("t_PALRequest").Rows.Add(dr)
            da.Update(ds, "t_PALRequest")
            _ID = ds.Tables("t_PALRequest").Rows(0).Item("PALRequestID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Public Function Get_PAL_Requests() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select pl.PALRequestID as ID, p.LastName + ', ' + p.FirstName as Employee, d.ComboItem as Department, pl.DateCreated, pl.TotalPALHours, pl.TotalSSLBHours, pl.TotalUnpaidHours, Case when Scheduled = 1 then 'Scheduled' else 'UnScheduled' end as [Leave Type] from t_PalRequest pl inner join t_Personnel p on pl.PersonnelID = p.PersonnelID inner join t_ComboItems d on pl.DepartmentID = d.ComboItemID where pl.ManagerApproved = 0 order by p.LastName, p.FirstName"
        Return ds
    End Function


    Public Function Get_Dept_PAL_Requests(ByVal PersonnelID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select pl.PALRequestID as ID, p.LastName + ', ' + p.FirstName as Employee, d.ComboItem as Department, pl.DateCreated, pl.TotalPALHours, pl.TotalSSLBHours, pl.TotalUnpaidHours, Case when Scheduled = 1 then 'Scheduled' else 'UnScheduled' end as [Leave Type], (Select min(StartDate) from t_PalRequestDates where palrequestid = pl.PalRequestID) as StartDate, (Select max(EndDate) from t_PalRequestDates where palrequestid = pl.PalRequestID) as EndDate from t_PalRequest pl inner join t_Personnel p on pl.PersonnelID = p.PersonnelID inner join t_ComboItems d on pl.DepartmentID = d.ComboItemID where pl.ManagerApproved = 0 and pl.DepartmentID in (Select DepartmentID from t_Personnel2Dept where personnelid = " & PersonnelID & " and active = 1 and isManager = 1) order by p.LastName, p.FirstName "
        Return ds
    End Function

    Public Function Check_PAL_Balance(ByVal PersonnelID As Integer) As Double
        Dim palHours As Double = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Sum(AvailablePAL) as PALBalance from v_PALBalances where personnelid = " & PersonnelID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                palHours = dread("PALBalance")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return palHours
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
            oEvents.KeyField = "PALRequestID"
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

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
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

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property ManagerApproved() As Integer
        Get
            Return _ManagerApproved
        End Get
        Set(ByVal value As Integer)
            _ManagerApproved = value
        End Set
    End Property

    Public Property ManagerID() As Integer
        Get
            Return _ManagerID
        End Get
        Set(ByVal value As Integer)
            _ManagerID = value
        End Set
    End Property

    Public Property DateApproved() As String
        Get
            Return _DateApproved
        End Get
        Set(ByVal value As String)
            _DateApproved = value
        End Set
    End Property

    Public Property TotalPALHours() As Decimal
        Get
            Return _TotalPALHours
        End Get
        Set(ByVal value As Decimal)
            _TotalPALHours = value
        End Set
    End Property

    Public Property TotalSSLBHours() As Decimal
        Get
            Return _TotalSSLBHours
        End Get
        Set(ByVal value As Decimal)
            _TotalSSLBHours = value
        End Set
    End Property

    Public Property TotalUnpaidHours() As Decimal
        Get
            Return _TotalUnpaidHours
        End Get
        Set(ByVal value As Decimal)
            _TotalUnpaidHours = value
        End Set
    End Property

    Public Property PALRequestID() As Integer
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

    Public Property Scheduled() As Boolean
        Get
            Return _Scheduled
        End Get
        Set(ByVal value As Boolean)
            _Scheduled = value
        End Set
    End Property

    Public Property DoctorsNote() As Boolean
        Get
            Return _DoctorsNote
        End Get
        Set(ByVal value As Boolean)
            _DoctorsNote = value
        End Set
    End Property
End Class
