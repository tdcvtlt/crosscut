Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnelSecurityRequest2Group
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RequestID As Integer = 0
    Dim _GroupID As String = ""
    Dim _GroupTypeID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelSecurityRequest2Group where Request2GroupID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelSecurityRequest2Group where Request2GroupID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelSecurityRequest2Group")
            If ds.Tables("t_PersonnelSecurityRequest2Group").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelSecurityRequest2Group").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RequestID") Is System.DBNull.Value) Then _RequestID = dr("RequestID")
        If Not (dr("GroupID") Is System.DBNull.Value) Then _GroupID = dr("GroupID")
        If Not (dr("GroupTypeID") Is System.DBNull.Value) Then _GroupTypeID = dr("GroupTypeID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelSecurityRequest2Group where Request2GroupID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelSecurityRequest2Group")
            If ds.Tables("t_PersonnelSecurityRequest2Group").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelSecurityRequest2Group").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelSecurityRequest2GroupInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RequestID", SqlDbType.int, 0, "RequestID")
                da.InsertCommand.Parameters.Add("@GroupID", SqlDbType.varchar, 0, "GroupID")
                da.InsertCommand.Parameters.Add("@GroupTypeID", SqlDbType.int, 0, "GroupTypeID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Request2GroupID", SqlDbType.Int, 0, "Request2GroupID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelSecurityRequest2Group").NewRow
            End If
            Update_Field("RequestID", _RequestID, dr)
            Update_Field("GroupID", _GroupID, dr)
            Update_Field("GroupTypeID", _GroupTypeID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PersonnelSecurityRequest2Group").Rows.Count < 1 Then ds.Tables("t_PersonnelSecurityRequest2Group").Rows.Add(dr)
            da.Update(ds, "t_PersonnelSecurityRequest2Group")
            _ID = ds.Tables("t_PersonnelSecurityRequest2Group").Rows(0).Item("Request2GroupID")
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
            oEvents.KeyField = "Request2GroupID"
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


    Public Function get_Groups(ByVal ID As Integer, ByVal filter As Integer) As String
        Dim groups As String = ""
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select GroupID from t_PersonnelSecurityRequest2Group where RequestID = " & ID & " and GroupTypeID = " & filter & " and Active = 1"
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Do While dread.Read
                If groups = "" Then
                    groups = dread("GroupID")
                Else
                    groups = groups & "|" & dread("GroupID")
                End If
            Loop
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return groups
    End Function

    Public Function Get_Group_Display(ByVal ID As Integer, ByVal filter As String) As String
        Dim grps As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If filter = "CRMS" Then
                cm.CommandText = "Select pg.GroupName from t_PersonnelSecurityRequest2Group r inner join t_PersonnelGroup pg on r.GroupID = pg.PersonnelGroupID inner join t_CombOitems gt on r.GroupTypeID = gt.ComboItemID where r.RequestID = " & ID & " and r.Active = 1 and gt.CombOitem = 'CRMS' order by pg.GroupName asc"
            Else
                Dim oCombo As New clsComboItems
                cm.CommandText = "Select r.GroupID as GroupName from t_personnelsecurityRequest2Group r where RequestID = " & ID & " and active = 1 and r.GroupTypeID = " & oCombo.Lookup_ID("ITSecRequestGroupType", filter) & " order by r.GroupID asc"
                oCombo = Nothing
            End If
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If grps = "" Then
                        grps = Replace(dread("GroupName"), "CN=", "")
                    Else
                        grps = grps & ", " & Replace(dread("GroupName"), "CN=", "")
                    End If
                Loop
            Else
                grps = "N/A"
            End If
            dread.Close()
        Catch ex As Exception

        End Try
        Return grps
    End Function

    Public Function Get_Groups_By_Type(ByVal ID As Integer, ByVal filter As String) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ID")
        dt.Columns.Add("GroupTypeID")
        dt.Columns.Add("GroupID")
        If cn.State <> ConnectionState.Open Then cn.Open()
        If filter = "ALL" Then
            cm.CommandText = "Select * from t_PersonnelSecurityRequest2Group where RequestID = " & ID
        Else
            Dim oCombo As New clsComboItems
            cm.CommandText = "Select * from t_PersonnelSecurityRequest2Group where RequestID = " & ID & " and GroupTypeID = " & oCombo.Lookup_ID("ITSecRequestGroupType", filter) & ""
            oCombo = Nothing
        End If
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Dim dr As DataRow
            Do While dread.Read
                dr = dt.NewRow
                dr("ID") = dread("Request2GroupID")
                dr("GroupTypeID") = dread("GroupTypeID")
                dr("GroupID") = dread("GroupID")
                dt.Rows.Add(dr)
            Loop
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return dt
    End Function
    Public Property RequestID() As Integer
        Get
            Return _RequestID
        End Get
        Set(ByVal value As Integer)
            _RequestID = value
        End Set
    End Property

    Public Property GroupID() As String
        Get
            Return _GroupID
        End Get
        Set(ByVal value As String)
            _GroupID = value
        End Set
    End Property

    Public Property GroupTypeID() As Integer
        Get
            Return _GroupTypeID
        End Get
        Set(ByVal value As Integer)
            _GroupTypeID = value
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

    Public Property Request2GroupID() As Integer
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

    Public Property ErrID() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property
End Class
