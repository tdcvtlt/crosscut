Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadLists
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _DateRevoked As String = ""
    Dim _RevokedByID As Integer = 0
    Dim _VendorID As Integer = 0
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadLists where LeadListID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadLists where LeadListID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadLists")
            If ds.Tables("t_LeadLists").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadLists").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateRevoked") Is System.DBNull.Value) Then _DateRevoked = dr("DateRevoked")
        If Not (dr("RevokedByID") Is System.DBNull.Value) Then _RevokedByID = dr("RevokedByID")
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadLists where LeadListID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadLists")
            If ds.Tables("t_LeadLists").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadLists").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadListsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateRevoked", SqlDbType.datetime, 0, "DateRevoked")
                da.InsertCommand.Parameters.Add("@RevokedByID", SqlDbType.int, 0, "RevokedByID")
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.int, 0, "VendorID")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@LeadListID", SqlDbType.Int, 0, "LeadListID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadLists").NewRow
            End If
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateRevoked", _DateRevoked, dr)
            Update_Field("RevokedByID", _RevokedByID, dr)
            Update_Field("VendorID", _VendorID, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_LeadLists").Rows.Count < 1 Then ds.Tables("t_LeadLists").Rows.Add(dr)
            da.Update(ds, "t_LeadLists")
            _ID = ds.Tables("t_LeadLists").Rows(0).Item("LeadListID")
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
            oEvents.KeyField = "LeadListID"
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

    Public Function get_Qualification_Headers() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Name from sysColumns where id=OBJECT_ID('v_LeadQualifications') order by name asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function get_Qualification_Values(ByVal field As String) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Distinct([" & field & "]) as Value from v_LeadQualifications order by [" & field & "] asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_Lead_Count(ByVal newLeads As Boolean, ByVal sSQL As String) As Integer
        Dim leads As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandTimeout = 0
            If newLeads Then
                cm.CommandText = "Select Count(Distinct(l.LeadID)) as Leads from v_Leads l left outer join t_Lead2List ll on l.LeadID = ll.LeadID where ll.LeadID is null and (l.DNC = 0 or l.DNC IS NULL) and " & sSQL
            Else
                cm.CommandText = "Select Count(Distinct(l.LeadID)) as Leads from v_Leads l inner join t_Lead2List ll on l.LeadID = ll.LeadID where (l.DNC = 0 or l.DNC is NULL) and l.LeadID not in (Select x.LeadID from t_Lead2List x inner join t_LeadLists lx on x.ListID = lx.LeadListID where lx.DateCreated > '" & System.DateTime.Now.AddDays(-30) & "') and " & sSQL
            End If
            If newLeads Then
                cm.CommandText = "Select Count(Distinct(l.phonenumber)) as Leads from v_Leads l where phonenumber not in (select phonenumber from t_Leads l inner join t_Lead2List ll on ll.leadid = l.leadid) and (l.DNC = 0 or l.DNC IS NULL) and " & sSQL
            Else
                cm.CommandText = "Select Count(Distinct(l.PhoneNumber)) as Leads from v_Leads l inner join t_Lead2List ll on l.LeadID = ll.LeadID where (l.DNC = 0 or l.DNC is NULL) and l.PhoneNumber not in (Select l2.phonenumber from t_Leads l2 inner join t_Lead2List x on x.leadid = l2.leadid inner join t_LeadLists lx on x.ListID = lx.LeadListID where (lx.DateCreated > '" & System.DateTime.Now.AddDays(-30) & "' and lx.DateRevoked is NULL)) and " & sSQL
            End If
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                leads = dread("Leads")
            Else
                leads = 0
            End If
        Catch ex As Exception
            _Err = ex.Message
            Return -1
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return leads
    End Function

    Public Function Get_Lead_Lists() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select l.LeadListID as ID, l.DateCreated, l.DateRevoked, v.Vendor, (Select Count(LeadID) from t_Lead2List where listID = l.LeadListID) as Leads, l.Description from t_LeadLists l inner join t_Vendor v on l.VendorID = v.VendorID order by l.DateCreated desc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property DateRevoked() As String
        Get
            Return _DateRevoked
        End Get
        Set(ByVal value As String)
            _DateRevoked = value
        End Set
    End Property

    Public Property RevokedByID() As Integer
        Get
            Return _RevokedByID
        End Get
        Set(ByVal value As Integer)
            _RevokedByID = value
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property LeadListID() As Integer
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

