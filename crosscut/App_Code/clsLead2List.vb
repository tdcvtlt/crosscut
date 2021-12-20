Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLead2List
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LeadID As Integer = 0
    Dim _ListID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Lead2List where Lead2ListID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Lead2List where Lead2ListID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Lead2List")
            If ds.Tables("t_Lead2List").Rows.Count > 0 Then
                dr = ds.Tables("t_Lead2List").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LeadID") Is System.DBNull.Value) Then _LeadID = dr("LeadID")
        If Not (dr("ListID") Is System.DBNull.Value) Then _ListID = dr("ListID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Lead2List where Lead2ListID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Lead2List")
            If ds.Tables("t_Lead2List").Rows.Count > 0 Then
                dr = ds.Tables("t_Lead2List").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Lead2ListInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LeadID", SqlDbType.int, 0, "LeadID")
                da.InsertCommand.Parameters.Add("@ListID", SqlDbType.int, 0, "ListID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Lead2ListID", SqlDbType.Int, 0, "Lead2ListID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Lead2List").NewRow
            End If
            Update_Field("LeadID", _LeadID, dr)
            Update_Field("ListID", _ListID, dr)
            If ds.Tables("t_Lead2List").Rows.Count < 1 Then ds.Tables("t_Lead2List").Rows.Add(dr)
            da.Update(ds, "t_Lead2List")
            _ID = ds.Tables("t_Lead2List").Rows(0).Item("Lead2ListID")
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
            oEvents.KeyField = "Lead2ListID"
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

    Public Function Build_List(ByVal newLeads As Integer, ByVal oldLeads As Integer, ByVal sSQL As String, ByVal listID As Integer) As Boolean
        Dim bBuilt As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()

            Dim sQL As String = ""
            If newLeads > 0 Then
                sQL = "Select Top " & newLeads & " l.LeadID, " & listID & " as ListID from t_Leads l Left outer join t_Lead2List ll on l.LeadID = ll.LeadID " & _
                        "where ll.LeadID is null and (l.DNC = 0 or l.DNC IS NULL) and " & sSQL
                sQL = "Select Top " & newLeads & " * from ((select top 1 LeadID from t_Leads where PhoneNumber = l.PhoneNumber) as LeadID, " & listID & " as ListID from (select distinct phonenumber from t_Leads where (dnc is null or dnc=0)) l) a where " & sSQL
                sQL = "Select top " & newLeads & " * from (select (select top 1 LeadID from t_Leads where PhoneNumber = a.PhoneNumber) as LeadID, " & listID & " as ListID from (select distinct phonenumber from v_Leads l where (dnc is null or dnc = 0) and PhoneNumber not in (select PhoneNumber from t_Leads b inner join t_Lead2List c on c.LeadID = b.LeadID) and " & sSQL & " ) a )d"
            End If
            If oldLeads > 0 Then
                If sQL <> "" Then
                    sQL = sQL & " UNION "
                End If
                'sQL = sQL & "Select top " & oldLeads & " a.LeadID, " & listID & " as ListID from t_Leads a inner join t_Lead2List b on a.LeadID = b.LeadID " & _
                '        "where a.LeadID not in (Select x.leadID from t_Lead2List x inner join t_leadLists y on x.ListID = y.LeadListID where y.DateCreated > '" & System.DateTime.Now.AddDays(-30) & "')" & _
                '        "and (a.DNC = 0 or a.DNC IS NULL) and " & sSQL
                sQL = sQL & "Select top " & oldLeads & " * from ( select (select top 1 LeadID from t_Leads where PhoneNumber = a.PhoneNumber) as LeadID, " & listID & " as ListID from (select distinct phonenumber from v_Leads l where (dnc is null or dnc = 0) and PhoneNumber not in (select PhoneNumber from t_Leads b inner join t_Lead2List c on c.LeadID = b.LeadID inner join t_LeadLists ll on ll.LeadListID = c.ListID  where (ll.datecreated > '" & System.DateTime.Now.AddDays(-30) & "' and ll.DateRevoked is null)) and " & sSQL & " ) a )d"
            End If
            cm.CommandTimeout = 0
            cm.CommandText = "Insert Into t_Lead2List(LeadID, ListID) " & sQL
            cm.ExecuteNonQuery()
            bBuilt = True
        Catch ex As Exception
            _Err = ex.Message
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bBuilt
    End Function

    Public Property LeadID() As Integer
        Get
            Return _LeadID
        End Get
        Set(ByVal value As Integer)
            _LeadID = value
        End Set
    End Property

    Public Property ListID() As Integer
        Get
            Return _ListID
        End Get
        Set(ByVal value As Integer)
            _ListID = value
        End Set
    End Property

    Public Property Lead2ListID() As Integer
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
