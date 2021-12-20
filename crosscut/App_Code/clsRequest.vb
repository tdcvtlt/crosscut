Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRequest
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _EnteredByID As Integer = 0
    Dim _EntryDate As String = ""
    Dim _RequestAreaID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _AssignedToID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _Subject As String = ""
    Dim _Description As String = ""
    Dim _CategoryID As String = ""
    Dim _IssuedID As Integer = 0
    Dim _StartDate As String = ""
    Dim _StartTime As String = ""
    Dim _EndDate As String = ""
    Dim _EndTime As String = ""
    Dim _Priority As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Request where RequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Request where RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Request")
            If ds.Tables("t_Request").Rows.Count > 0 Then
                dr = ds.Tables("t_Request").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("EnteredByID") Is System.DBNull.Value) Then _EnteredByID = dr("EnteredByID")
        If Not (dr("EntryDate") Is System.DBNull.Value) Then _EntryDate = dr("EntryDate")
        If Not (dr("RequestAreaID") Is System.DBNull.Value) Then _RequestAreaID = dr("RequestAreaID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("AssignedToID") Is System.DBNull.Value) Then _AssignedToID = dr("AssignedToID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("Subject") Is System.DBNull.Value) Then _Subject = dr("Subject")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("CategoryID") Is System.DBNull.Value) Then _CategoryID = dr("CategoryID")
        If Not (dr("IssuedID") Is System.DBNull.Value) Then _IssuedID = dr("IssuedID")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("StartTime") Is System.DBNull.Value) Then _StartTime = dr("StartTime")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("EndTime") Is System.DBNull.Value) Then _EndTime = dr("EndTime")
        If Not (dr("Priority") Is System.DBNull.Value) Then _Priority = dr("Priority")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("KeyValue") Is System.DBNull.Value) Then _KeyValue = dr("KeyValue")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Request where RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Request")
            If ds.Tables("t_Request").Rows.Count > 0 Then
                dr = ds.Tables("t_Request").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RequestInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@EnteredByID", SqlDbType.Int, 0, "EnteredByID")
                da.InsertCommand.Parameters.Add("@EntryDate", SqlDbType.DateTime, 0, "EntryDate")
                da.InsertCommand.Parameters.Add("@RequestAreaID", SqlDbType.Int, 0, "RequestAreaID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.Int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@AssignedToID", SqlDbType.Int, 0, "AssignedToID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@Subject", SqlDbType.VarChar, 0, "Subject")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.VarChar, 0, "Description")
                da.InsertCommand.Parameters.Add("@CategoryID", SqlDbType.VarChar, 0, "CategoryID")
                da.InsertCommand.Parameters.Add("@IssuedID", SqlDbType.Int, 0, "IssuedID")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.DateTime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@StartTime", SqlDbType.VarChar, 0, "StartTime")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.DateTime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@EndTime", SqlDbType.VarChar, 0, "EndTime")
                da.InsertCommand.Parameters.Add("@Priority", SqlDbType.Int, 0, "Priority")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.VarChar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.Int, 0, "KeyValue")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RequestID", SqlDbType.Int, 0, "RequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Request").NewRow
            End If
            Update_Field("EnteredByID", _EnteredByID, dr)
            Update_Field("EntryDate", _EntryDate, dr)
            Update_Field("RequestAreaID", _RequestAreaID, dr)
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("AssignedToID", _AssignedToID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("Subject", _Subject, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("CategoryID", _CategoryID, dr)
            Update_Field("IssuedID", _IssuedID, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("StartTime", _StartTime, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("EndTime", _EndTime, dr)
            Update_Field("Priority", _Priority, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            If ds.Tables("t_Request").Rows.Count < 1 Then ds.Tables("t_Request").Rows.Add(dr)
            da.Update(ds, "t_Request")
            _ID = ds.Tables("t_Request").Rows(0).Item("RequestID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "RequestID"
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

    Public Function List_maint_Requests(ByVal roomID As Integer) As SQLDataSource
        Dim ds As New sqldatasource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Top 50 r.RequestID, r.EntryDate, r.Subject, ts.ComboItem as Status from t_Request r inner join t_ComboItems ts on r.StatusID = ts.CombOitemID where r.RoomID = " & roomID & " order by entrydate desc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Maint_Reps(ByVal filter As String, Optional ByVal persID As Integer = 0) As SqlDataSource
        Dim ds As New sqlDataSOurce
        ds.ConnectionString = Resources.Resource.cns
        Try
            If filter = "MC" Then
                ds.SelectCommand = "Select Distinct(p.PersonnelID), p.FirstName + ' ' + p.LastName as Personnel from t_Personnel2Dept pd inner join t_Personnel p on pd.PersonnelID = p.PersonnelID inner join t_ComboItems ps on p.StatusID = ps.ComboItemID inner join t_ComboItems d on pd.DepartmentID = d.ComboItemID where ps.ComboItem = 'Active' and pd.Active = '1' and d.ComboItem in ('Mastercorp') or p.personnelid = '" & persID & "' order by personnel asc"
            ElseIf filter = "All" Then
                ds.SelectCommand = "Select Distinct(p.PersonnelID), p.FirstName + ' ' + p.LastName as Personnel from t_Personnel2Dept pd inner join t_Personnel p on pd.PersonnelID = p.PersonnelID inner join t_ComboItems ps on p.StatusID = ps.ComboItemID inner join t_ComboItems d on pd.DepartmentID = d.ComboItemID where ps.ComboItem = 'Active' and pd.Active = '1' and d.ComboItem in ('Q&A Dispatch/Inventory','Utility','Q&A Maintenance','Grounds','Housekeeping','Mastercorp','Security','Support Services') or p.personnelid = '" & persID & "'  order by personnel asc"
            Else
                ds.SelectCommand = "Select Distinct(p.PersonnelID), p.FirstName + ' ' + p.LastName as Personnel from t_Personnel2Dept pd inner join t_Personnel p on pd.PersonnelID = p.PersonnelID inner join t_ComboItems ps on p.StatusID = ps.ComboItemID inner join t_ComboItems d on pd.DepartmentID = d.ComboItemID where ps.ComboItem = 'Active' and pd.Active = '1' and d.ComboItem in ('Q&A Dispatch/Inventory','Utility','Q&A Maintenance','Grounds','Housekeeping','Security','Support Services') or p.personnelid = '" & persID & "' order by personnel asc"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function List_Categories(ByVal filterLvl As Integer, Optional ByVal filter1 As String = "", Optional ByVal filter2 As String = "", Optional ByVal filter3 As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Select Case filterLvl
                Case 0
                    ds.SelectCommand = "Select Distinct RTrim(ITMCLSCD) As Category from t_IV00101 order by RTRIM(ITMCLSCD) ASC"
                Case 1
                    ds.SelectCommand = "Select Distinct USCATVLS_1 As Category from t_IV00101 where RTRIM(ITMCLSCD) = '" & RTrim(filter1) & "' and USCATVLS_1 <> '' Order by USCATVLS_1 ASC"
                Case 2
                    ds.SelectCommand = "Select Distinct USCATVLS_2 As Category from t_IV00101 where RTRIM(ITMCLSCD) = '" & RTrim(filter1) & "' and RTRIM(USCATVLS_1) = '" & RTrim(filter2) & "' Order By USCATVLS_2 ASC"
                Case 3
                    ds.SelectCommand = "Select Distinct USCATVLS_3 As Category from t_IV00101 where RTRIM(ITMCLSCD) = '" & RTrim(filter1) & "' and RTRIM(USCATVLS_1) = '" & RTrim(filter2) & "' and RTRIM(USCATVLS_2) = '" & RTrim(filter3) & "' Order By USCATVLS_3 ASC"
            End Select
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Search_Parts(ByVal filterLvl As Integer, Optional ByVal filter1 As String = "", Optional ByVal filter2 As String = "", Optional ByVal filter3 As String = "", Optional ByVal filter4 As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            Select Case filterLvl
                Case 0
                    ds.SelectCommand = "Select ITEMNMBR, ITEMDESC from t_IV00101  ORDER BY ITEMNMBR ASC" 'where ItemType = '1' ORDER BY ITEMNMBR ASC"
                Case 1
                    ds.SelectCommand = "Select ITEMNMBR, ITEMDESC from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filter1) & "' ORDER BY ITEMNMBR ASC" ' and ITEMTYPE = '1' 
                Case 2
                    ds.SelectCommand = "Select ITEMNMBR, ITEMDESC from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filter1) & "' and RTRIM(USCATVLS_1) = '" & Trim(filter2) & "' ORDER BY ITEMNMBR ASC" 'and ITEMTYPE = '1' 
                Case 3
                    ds.SelectCommand = "Select ITEMNMBR, ITEMDESC from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filter1) & "' and RTRIM(USCATVLS_1) = '" & Trim(filter2) & "' and RTRIM(USCATVLS_2) = '" & Trim(filter3) & "' ORDER BY ITEMNMBR ASC" ' and ITEMTYPE = '1' 
                Case 4
                    ds.SelectCommand = "Select ITEMNMBR, ITEMDESC from t_IV00101 where RTRIM(ITMCLSCD) = '" & Trim(filter1) & "' and RTRIM(USCATVLS_1) = '" & Trim(filter2) & "' and RTRIM(USCATVLS_2) = '" & Trim(filter3) & "' and RTRIM(USCATVLS_3) = '" & Trim(filter4) & "' ORDER BY ITEMNMBR ASC" ' and ITEMTYPE = '1'
            End Select
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Print_Request(ByVal requestID As Integer) As String
        Dim req As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "SELECT q.RequestID, q.EntryDate, p.firstname + ' ' + p.lastname as Personnel, ram.dateallocated, Case when q.Keyfield = 'RoomID' then pros.LastName + ',' + pros.FirstName else 'N/A' end as Prospect, res.CheckInDate, res.CheckOutDate, Case when q.KeyField = 'RoomID' then r.RoomNumber else pm.Name end as ProbLoc, q.Description, l.ComboItem as RequestArea, Case when r.Phone is null then 'N/A' else r.Phone end as Phone, q.KeyField, " & _
                                "Case when q.KeyField = 'RoomID' then (select lastname + ', ' + firstname from t_Prospect where prospectid in " & _
                                "(select prospectid from t_Reservations where reservationid in " & _
                                "(select reservationid from t_Roomallocationmatrix where datediff(d,dateallocated,q.entrydate) = 0 and roomid = q.roomid))) else 'NA' end as RequestingPros " & _
                                "FROM t_Request q " & _
                                "LEFT OUTER JOIN t_Personnel p on p.PersonnelID = q.AssignedToID " & _
                                "left outer JOIN t_ComboItems l on l.ComboItemID = q.RequestAreaID " & _
                                "left outer JOIN t_Room r on r.RoomID = q.KeyValue " & _
                                "left outer JOIN t_PMBuilding pm on q.KeyValue = pm.PMBuildingID " & _
                                "left outer JOIN " & _
                                    "(select * from t_RoomAllocationMatrix where datediff(d,dateallocated, getdate()) = 0) ram on r.RoomID = ram.RoomID " & _
                                "left outer JOIN t_Reservations res on ram.ReservationID = res.ReservationID " & _
                                "left outer JOIN t_Prospect pros on res.ProspectID = pros.ProspectID " & _
                                "WHERE q.RequestID = '" & requestID & "'"
            dread = cm.ExecuteReader
            dread.Read()
            req = "<Table>"
            req = req & "<tr>"
            req = req & "<td colspan = '2'><b><font size = 4>MAINTENANCE REQUEST</font></b></td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td><b>TICKET NUMBER:</b></td>"
            req = req & "<td>" & dread("RequestID") & "</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td><b>DATE REQUESTED:</b></td>"
            req = req & "<td>" & dread("EntryDate") & "</td>"
            req = req & "</tr>"
            req = req & "</table>"
            req = req & "<hr class = 'linethin'>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td><b>Assigned To:</b></td>"
            req = req & "<td>" & dread("Personnel") & "</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td><b>Generated By:</b></td>"
            req = req & "<td>" & dread("RequestArea") & "</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            If dread("KeyField") = "RoomID" Then
                req = req & "<td><b>Room/Phone:</b></td>"
                req = req & "<td>" & dread("ProbLoc") & " / " & dread("Phone") & "</td>"
            Else
                req = req & "<td><b>Location:</b></td>"
                req = req & "<td>" & dread("ProbLoc") & "</td>"
            End If
            req = req & "</tr>"
            req = req & "</table>"
            req = req & "</td>"
            req = req & "<td>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td><b>Requesting Guest:</b></td>"
            req = req & "<td>" & dread("RequestingPros") & "</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td><B>Current Guest:</b></td>"
            req = req & "<td>" & dread("Prospect") & "</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td><B>Guest Stay:</b></td>"
            req = req & "<td>" & dread("CheckInDate") & " - " & dread("CheckOutDate") & "</td>"
            req = req & "</tr>"
            req = req & "</table>"
            req = req & "</td>"
            req = req & "</tr>"
            req = req & "</table>"
            req = req & "<hr class= 'linethin'>"
            req = req & "<table>"
            req = req & "<tr><td><b>Description:</b></td></tr>"
            req = req & "<tr><td><font size = '12'>" & dread("Description") & "</font></td></tr>"
            req = req & "</table>"
            req = req & "<hr class='linethin'>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td><b>TIME IN:</b></td>"
            req = req & "<td>__________</td>"
            req = req & "<td><b>TIME OUT:</b></td>"
            req = req & "<td>__________</td>"
            req = req & "</tr>"
            req = req & "</table>"
            'NOTES
            req = req & "<hr class='linethin'>"
            req = req & "<b>Notes:</b>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td><b><u>Note</u></b></td>"
            req = req & "<td><b><u>User Name</u></b></td>"
            req = req & "<td><b><u>Note Date</u></b></td>"
            req = req & "</tr>"
            dread.Close()
            cm.CommandText = "Select n.Note, p.username, n.DateCreated from t_Note n left outer join t_Personnel p on n.CreatedByID = p.PersonnelID where n.KeyField = 'RequestID' and n.KeyValue = " & requestID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    req = req & "<tr>"
                    req = req & "<td>" & dread("Note") & "</td>"
                    req = req & "<td>" & dread("UserName") & "</td>"
                    req = req & "<td>" & dread("DateCreated") & "</td>"
                    req = req & "</tr>"
                Loop
            End If
            req = req & "</table>"
            req = req & "<hr class='linethin'>"
            req = req & "<b>Items:</b>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td><u>QTY</u></td>"
            req = req & "<td><u>DESCRIPTION</u></td>"
            req = req & "<td><u>COMMENTS</u></td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>_______</td>"
            req = req & "<td>______________________________</td>"
            req = req & "<td>______________________________</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>_______</td>"
            req = req & "<td>______________________________</td>"
            req = req & "<td>______________________________</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>_______</td>"
            req = req & "<td>______________________________</td>"
            req = req & "<td>______________________________</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>_______</td>"
            req = req & "<td>______________________________</td>"
            req = req & "<td>______________________________</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>_______</td>"
            req = req & "<td>______________________________</td>"
            req = req & "<td>______________________________</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>_______</td>"
            req = req & "<td>______________________________</td>"
            req = req & "<td>______________________________</td>"
            req = req & "</tr>"
            req = req & "</table>"
            req = req & "<hr class='linethin'>"
            req = req & "<b>Completion:</b>"
            req = req & "<table>"
            req = req & "<tr>"
            req = req & "<td>Completed By:</td>"
            req = req & "<td>_______________________</td>"
            req = req & "<td>Signature:</td>"
            req = req & "<td>_______________________</td>"
            req = req & "</tr>"
            req = req & "<tr>"
            req = req & "<td>Manager:</td>"
            req = req & "<td>_______________________</td>"
            req = req & "<td>Signature:</td>"
            req = req & "<td>_______________________</td>"
            req = req & "</tr>"
            req = req & "</table>"
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            req = "Error" & _Err
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return req
    End Function

    Public Property EnteredByID() As Integer
        Get
            Return _EnteredByID
        End Get
        Set(ByVal value As Integer)
            _EnteredByID = value
        End Set
    End Property

    Public Property Priority As Integer
        Get
            Return _Priority
        End Get
        Set(value As Integer)
            _Priority = value
        End Set
    End Property

    Public Property EntryDate() As String
        Get
            Return _EntryDate
        End Get
        Set(ByVal value As String)
            _EntryDate = value
        End Set
    End Property

    Public Property RequestAreaID() As Integer
        Get
            Return _RequestAreaID
        End Get
        Set(ByVal value As Integer)
            _RequestAreaID = value
        End Set
    End Property

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
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

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property CategoryID() As String
        Get
            Return _CategoryID
        End Get
        Set(ByVal value As String)
            _CategoryID = value
        End Set
    End Property

    Public Property IssuedID() As Integer
        Get
            Return _IssuedID
        End Get
        Set(ByVal value As Integer)
            _IssuedID = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property StartTime() As String
        Get
            Return _StartTime
        End Get
        Set(ByVal value As String)
            _StartTime = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
        End Set
    End Property

    Public Property EndTime() As String
        Get
            Return _EndTime
        End Get
        Set(ByVal value As String)
            _EndTime = value
        End Set
    End Property

    Public Property RequestID() As Integer
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
    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property
    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property
End Class
