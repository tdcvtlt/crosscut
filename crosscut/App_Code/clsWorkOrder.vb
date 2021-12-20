Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsWorkOrder
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DateEntered As String = ""
    Dim _RequestedByID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _WaitingOnID As Integer = 0
    Dim _Subject As String = ""
    Dim _RequestTypeID As Integer = 0
    Dim _PriorityLevelID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _SubLocationID As Integer = 0
    Dim _DepartmentID As Integer = 0
    Dim _AssignedToID As Integer = 0
    Dim _ProblemAreaID As Integer = 0
    Dim _Description As String = ""
    Dim _InstallationTypeID As Integer = 0
    Dim _Cost As Decimal = 0
    Dim _OldValue As String = ""
    Dim _NewValue As String = ""
    Dim _FirstName As String = ""
    Dim _LastName As String = ""
    Dim _SupervisorID As Integer = 0
    Dim _PhoneNumber As String = ""
    Dim _ReqDueDate As String = ""
    Dim _AssignDueDate As String = ""
    Dim _ResponsiblePartyID As Integer = 0
    Dim _Approved As String = ""
    Dim _ApprovedBy As Integer = 0
    Dim _DateApproved As String = ""
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
        cm = New SqlCommand("Select * from t_WorkOrder where WorkOrderID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_WorkOrder where WorkOrderID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_WorkOrder")
            If ds.Tables("t_WorkOrder").Rows.Count > 0 Then
                dr = ds.Tables("t_WorkOrder").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DateEntered") Is System.DBNull.Value) Then _DateEntered = dr("DateEntered")
        If Not (dr("RequestedByID") Is System.DBNull.Value) Then _RequestedByID = dr("RequestedByID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("WaitingOnID") Is System.DBNull.Value) Then _WaitingOnID = dr("WaitingOnID")
        If Not (dr("Subject") Is System.DBNull.Value) Then _Subject = dr("Subject")
        If Not (dr("RequestTypeID") Is System.DBNull.Value) Then _RequestTypeID = dr("RequestTypeID")
        If Not (dr("PriorityLevelID") Is System.DBNull.Value) Then _PriorityLevelID = dr("PriorityLevelID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("SubLocationID") Is System.DBNull.Value) Then _SubLocationID = dr("SubLocationID")
        If Not (dr("DepartmentID") Is System.DBNull.Value) Then _DepartmentID = dr("DepartmentID")
        If Not (dr("AssignedToID") Is System.DBNull.Value) Then _AssignedToID = dr("AssignedToID")
        If Not (dr("ProblemAreaID") Is System.DBNull.Value) Then _ProblemAreaID = dr("ProblemAreaID")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("InstallationTypeID") Is System.DBNull.Value) Then _InstallationTypeID = dr("InstallationTypeID")
        If Not (dr("Cost") Is System.DBNull.Value) Then _Cost = dr("Cost")
        If Not (dr("OldValue") Is System.DBNull.Value) Then _OldValue = dr("OldValue")
        If Not (dr("NewValue") Is System.DBNull.Value) Then _NewValue = dr("NewValue")
        If Not (dr("FirstName") Is System.DBNull.Value) Then _FirstName = dr("FirstName")
        If Not (dr("LastName") Is System.DBNull.Value) Then _LastName = dr("LastName")
        If Not (dr("SupervisorID") Is System.DBNull.Value) Then _SupervisorID = dr("SupervisorID")
        If Not (dr("PhoneNumber") Is System.DBNull.Value) Then _PhoneNumber = dr("PhoneNumber")
        If Not (dr("ReqDueDate") Is System.DBNull.Value) Then _ReqDueDate = dr("ReqDueDate")
        If Not (dr("AssignDueDate") Is System.DBNull.Value) Then _AssignDueDate = dr("AssignDueDate")
        If Not (dr("ResponsiblePartyID") Is System.DBNull.Value) Then _ResponsiblePartyID = dr("ResponsiblePartyID")
        If Not (dr("Approved") Is System.DBNull.Value) Then _Approved = dr("Approved")
        If Not (dr("ApprovedBy") Is System.DBNull.Value) Then _ApprovedBy = dr("ApprovedBy")
        If Not (dr("DateApproved") Is System.DBNull.Value) Then _DateApproved = dr("DateApproved")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_WorkOrder where WorkOrderID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_WorkOrder")
            If ds.Tables("t_WorkOrder").Rows.Count > 0 Then
                dr = ds.Tables("t_WorkOrder").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_WorkOrderInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DateEntered", SqlDbType.datetime, 0, "DateEntered")
                da.InsertCommand.Parameters.Add("@RequestedByID", SqlDbType.int, 0, "RequestedByID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@WaitingOnID", SqlDbType.int, 0, "WaitingOnID")
                da.InsertCommand.Parameters.Add("@Subject", SqlDbType.varchar, 0, "Subject")
                da.InsertCommand.Parameters.Add("@RequestTypeID", SqlDbType.int, 0, "RequestTypeID")
                da.InsertCommand.Parameters.Add("@PriorityLevelID", SqlDbType.int, 0, "PriorityLevelID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@SubLocationID", SqlDbType.int, 0, "SubLocationID")
                da.InsertCommand.Parameters.Add("@DepartmentID", SqlDbType.int, 0, "DepartmentID")
                da.InsertCommand.Parameters.Add("@AssignedToID", SqlDbType.int, 0, "AssignedToID")
                da.InsertCommand.Parameters.Add("@ProblemAreaID", SqlDbType.int, 0, "ProblemAreaID")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                da.InsertCommand.Parameters.Add("@InstallationTypeID", SqlDbType.int, 0, "InstallationTypeID")
                da.InsertCommand.Parameters.Add("@Cost", SqlDbType.money, 0, "Cost")
                da.InsertCommand.Parameters.Add("@OldValue", SqlDbType.varchar, 0, "OldValue")
                da.InsertCommand.Parameters.Add("@NewValue", SqlDbType.varchar, 0, "NewValue")
                da.InsertCommand.Parameters.Add("@FirstName", SqlDbType.varchar, 0, "FirstName")
                da.InsertCommand.Parameters.Add("@LastName", SqlDbType.varchar, 0, "LastName")
                da.InsertCommand.Parameters.Add("@SupervisorID", SqlDbType.int, 0, "SupervisorID")
                da.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.varchar, 0, "PhoneNumber")
                da.InsertCommand.Parameters.Add("@ReqDueDate", SqlDbType.varchar, 0, "ReqDueDate")
                da.InsertCommand.Parameters.Add("@AssignDueDate", SqlDbType.varchar, 0, "AssignDueDate")
                da.InsertCommand.Parameters.Add("@ResponsiblePartyID", SqlDbType.int, 0, "ResponsiblePartyID")
                da.InsertCommand.Parameters.Add("@Approved", SqlDbType.varchar, 0, "Approved")
                da.InsertCommand.Parameters.Add("@ApprovedBy", SqlDbType.int, 0, "ApprovedBy")
                da.InsertCommand.Parameters.Add("@DateApproved", SqlDbType.datetime, 0, "DateApproved")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@WorkOrderID", SqlDbType.Int, 0, "WorkOrderID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_WorkOrder").NewRow
            End If
            Update_Field("DateEntered", _DateEntered, dr)
            Update_Field("RequestedByID", _RequestedByID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("WaitingOnID", _WaitingOnID, dr)
            Update_Field("Subject", _Subject, dr)
            Update_Field("RequestTypeID", _RequestTypeID, dr)
            Update_Field("PriorityLevelID", _PriorityLevelID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("SubLocationID", _SubLocationID, dr)
            Update_Field("DepartmentID", _DepartmentID, dr)
            Update_Field("AssignedToID", _AssignedToID, dr)
            Update_Field("ProblemAreaID", _ProblemAreaID, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("InstallationTypeID", _InstallationTypeID, dr)
            Update_Field("Cost", _Cost, dr)
            Update_Field("OldValue", _OldValue, dr)
            Update_Field("NewValue", _NewValue, dr)
            Update_Field("FirstName", _FirstName, dr)
            Update_Field("LastName", _LastName, dr)
            Update_Field("SupervisorID", _SupervisorID, dr)
            Update_Field("PhoneNumber", _PhoneNumber, dr)
            Update_Field("ReqDueDate", _ReqDueDate, dr)
            Update_Field("AssignDueDate", _AssignDueDate, dr)
            Update_Field("ResponsiblePartyID", _ResponsiblePartyID, dr)
            Update_Field("Approved", _Approved, dr)
            Update_Field("ApprovedBy", _ApprovedBy, dr)
            Update_Field("DateApproved", _DateApproved, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_WorkOrder").Rows.Count < 1 Then ds.Tables("t_WorkOrder").Rows.Add(dr)
            da.Update(ds, "t_WorkOrder")
            _ID = ds.Tables("t_WorkOrder").Rows(0).Item("WorkOrderID")
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
            oEvents.KeyField = "WorkOrderID"
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

    Public Function List_WorkOrders(ByVal filter As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "" Then
                ds.SelectCommand = "select Workorderid as ID, DateEntered, DateApproved, s.comboitem as Status, Subject, p.firstname + ' ' + p.lastname as RequestedBy, p2.firstname + ' ' + p2.lastname as AssignedTo from t_Workorder w inner join t_WODept2Party rp on w.ResponsiblePartyID = rp.ResponsiblePartyID inner join t_Personnel2Dept pd on rp.DepartmentID = pd.DepartmentID left outer join t_Comboitems s on s.comboitemid = w.statusid left outer join t_Personnel p on p.personnelid = w.requestedbyid left outer join t_Personnel p2 on p2.personnelid = w.assignedtoid where pd.Active = '1' and pd.PersonnelID = " & _UserID & " and s.ComboItem Not In ('Complete','Cancelled') and approved = '1' order by workorderid desc"
            Else
                'ds.SelectCommand = "select Workorderid as ID, DateEntered, s.comboitem as Status, Subject, p.firstname + ' ' + p.lastname as RequestedBy, p2.firstname + ' ' + p2.lastname as AssignedTo from t_Workorder w inner join t_WODept2Party rp on w.ResponsiblePartyID = rp.ResponsiblePartyID inner join t_Personnel2Dept pd on rp.DepartmentID = pd.DepartmentID left outer join t_Comboitems s on s.comboitemid = w.statusid left outer join t_Personnel p on p.personnelid = w.requestedbyid left outer join t_Personnel p2 on p2.personnelid = w.assignedtoid where pd.Active = '1' and pd.PersonnelID = " & _UserID & " and w.WorkOrderID = '" & filter & "' and approved = '1' order by workorderid desc"
                ds.SelectCommand = "select distinct Workorderid as ID, DateEntered, DateApproved, s.comboitem as Status, Subject, p.firstname + ' ' + p.lastname as RequestedBy, p2.firstname + ' ' + p2.lastname as AssignedTo from t_Workorder w inner join t_WODept2Party rp on w.ResponsiblePartyID = rp.ResponsiblePartyID inner join t_Personnel2Dept pd on rp.DepartmentID = pd.DepartmentID left outer join t_Comboitems s on s.comboitemid = w.statusid left outer join t_Personnel p on p.personnelid = w.requestedbyid left outer join t_Personnel p2 on p2.personnelid = w.assignedtoid where pd.Active = '1' and w.WorkOrderID = '" & filter & "' and approved = '1' order by workorderid desc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Resp_Party_Email(ByVal ID As Integer) As String
        Dim email As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select email from t_WODept2Party where responsiblepartyid = " & ID & ""
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                email = dread("Email")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return email
    End Function
    Public Function Get_My_WorkOrders(ByVal persID As Integer, ByVal approved As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select w.WorkOrderID, w.DateEntered, w.Subject, s.ComboItem as Status, p.FirstName + ' ' + p.Lastname as AssignedTo from t_WorkOrder w left outer join t_ComboItems s on w.StatusID = s.ComboItemID left outer join t_Personnel p on w.AssignedToID = p.PersonnelID where w.RequestedByID = " & persID & " and w.Approved = '" & approved & "' and s.ComboItem Not In ('Complete','Cancelled') order By dateentered desc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Dept_WorkOrders(ByVal persID As Integer, ByVal approved As String) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If approved = 1 Then
                ds.SelectCommand = "Select DISTINCT w.WorkOrderID, w.DateEntered, pe.FirstName + ' ' + pe.LastName AS EnteredBy, d.ComboItem as Department, w.Subject, s.ComboItem AS Status, p.FirstName + ' ' + p.LastName AS AssignedTo FROM t_Personnel2Dept pd INNER JOIN t_Personnel2Dept pd2 ON pd.DepartmentID = pd2.DepartmentID INNER JOIN t_WorkOrder w ON w.RequestedByID = pd2.PersonnelID left outer join t_ComboItems d on w.DepartmentID = d.CombOitemID LEFT OUTER JOIN t_ComboItems s ON w.StatusID = s.ComboItemID LEFT OUTER JOIN t_Personnel pe ON w.RequestedByID = pe.PersonnelID LEFT OUTER JOIN t_Personnel p ON w.AssignedToID = p.PersonnelID WHERE (pd.Active = '1' and pd2.Active = '1') AND (pd.PersonnelID = " & persID & ") and w.Approved = '1' and s.Comboitem not in ('Complete','Cancelled') order by w.workorderid desc"
            Else
                ds.SelectCommand = "Select DISTINCT w.WorkOrderID, w.DateEntered, pe.FirstName + ' ' + pe.LastName AS EnteredBy, d.CombOItem as Department, w.Subject, s.ComboItem AS Status FROM t_Personnel2Dept pd INNER JOIN t_Personnel2Dept pd2 ON pd.DepartmentID = pd2.DepartmentID INNER JOIN t_WorkOrder w ON w.RequestedByID = pd2.PersonnelID left outer join t_ComboItems d on w.DepartmentID = d.CombOitemID LEFT OUTER JOIN t_ComboItems s ON w.StatusID = s.ComboItemID LEFT OUTER JOIN t_Personnel pe ON w.RequestedByID = pe.PersonnelID LEFT OUTER JOIN t_Personnel p ON w.AssignedToID = p.PersonnelID WHERE (pd.Active = '1' and pd2.Active = '1') AND (pd.PersonnelID = " & persID & ") and w.Approved = '0' and pd.IsManager = '1' and s.ComboItem not in ('Complete','Cancelled') order by w.workorderid desc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property DateEntered() As String
        Get
            Return _DateEntered
        End Get
        Set(ByVal value As String)
            _DateEntered = value
        End Set
    End Property

    Public Property RequestedByID() As Integer
        Get
            Return _RequestedByID
        End Get
        Set(ByVal value As Integer)
            _RequestedByID = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property WaitingOnID() As Integer
        Get
            Return _WaitingOnID
        End Get
        Set(ByVal value As Integer)
            _WaitingOnID = value
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

    Public Property RequestTypeID() As Integer
        Get
            Return _RequestTypeID
        End Get
        Set(ByVal value As Integer)
            _RequestTypeID = value
        End Set
    End Property

    Public Property PriorityLevelID() As Integer
        Get
            Return _PriorityLevelID
        End Get
        Set(ByVal value As Integer)
            _PriorityLevelID = value
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

    Public Property SubLocationID() As Integer
        Get
            Return _SubLocationID
        End Get
        Set(ByVal value As Integer)
            _SubLocationID = value
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

    Public Property AssignedToID() As Integer
        Get
            Return _AssignedToID
        End Get
        Set(ByVal value As Integer)
            _AssignedToID = value
        End Set
    End Property

    Public Property ProblemAreaID() As Integer
        Get
            Return _ProblemAreaID
        End Get
        Set(ByVal value As Integer)
            _ProblemAreaID = value
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

    Public Property InstallationTypeID() As Integer
        Get
            Return _InstallationTypeID
        End Get
        Set(ByVal value As Integer)
            _InstallationTypeID = value
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

    Public Property OldValue() As String
        Get
            Return _OldValue
        End Get
        Set(ByVal value As String)
            _OldValue = value
        End Set
    End Property

    Public Property NewValue() As String
        Get
            Return _NewValue
        End Get
        Set(ByVal value As String)
            _NewValue = value
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

    Public Property SupervisorID() As Integer
        Get
            Return _SupervisorID
        End Get
        Set(ByVal value As Integer)
            _SupervisorID = value
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

    Public Property ReqDueDate() As String
        Get
            Return _ReqDueDate
        End Get
        Set(ByVal value As String)
            _ReqDueDate = value
        End Set
    End Property

    Public Property AssignDueDate() As String
        Get
            Return _AssignDueDate
        End Get
        Set(ByVal value As String)
            _AssignDueDate = value
        End Set
    End Property

    Public Property ResponsiblePartyID() As Integer
        Get
            Return _ResponsiblePartyID
        End Get
        Set(ByVal value As Integer)
            _ResponsiblePartyID = value
        End Set
    End Property

    Public Property Approved() As String
        Get
            Return _Approved
        End Get
        Set(ByVal value As String)
            _Approved = value
        End Set
    End Property

    Public Property ApprovedBy() As Integer
        Get
            Return _ApprovedBy
        End Get
        Set(ByVal value As Integer)
            _ApprovedBy = value
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

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property WorkOrderID() As Integer
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
End Class
