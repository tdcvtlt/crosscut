Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnel2Group
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelGroupID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Personnel2Group where PersonnelID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Personnel2Group where PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Personnel2Group")
            If ds.Tables("t_Personnel2Group").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel2Group").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelGroupID") Is System.DBNull.Value) Then _PersonnelGroupID = dr("PersonnelGroupID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Personnel2Group where PersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Personnel2Group")
            If ds.Tables("t_Personnel2Group").Rows.Count > 0 Then
                dr = ds.Tables("t_Personnel2Group").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Personnel2GroupInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelGroupID", SqlDbType.int, 0, "PersonnelGroupID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.Int, 0, "PersonnelID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Personnel2Group").NewRow
            End If
            Update_Field("PersonnelGroupID", _PersonnelGroupID, dr)
            If ds.Tables("t_Personnel2Group").Rows.Count < 1 Then ds.Tables("t_Personnel2Group").Rows.Add(dr)
            da.Update(ds, "t_Personnel2Group")
            _ID = ds.Tables("t_Personnel2Group").Rows(0).Item("PersonnelID")
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
            oEvents.KeyField = "PersonnelID"
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

    Public Function List_Groups(ByVal persID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Cast(pg.PersonnelID as varchar) + '-' + Cast(pg.PersonnelGroupID as varchar) as ID, g.GroupName from t_Personnel2Group pg inner join t_PersonnelGroup g on pg.PersonnelGroupID = g.PersonnelGroupID where pg.PersonnelID = " & persID & " order by g.GroupName asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Group_Members(ByVal groupID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select Cast(pg.PersonnelID as varchar) + '-' + Cast(pg.PersonnelGroupID as varchar) as ID, p.FirstName + ' ' + p.LastName as Personnel from t_Personnel2Group pg inner join t_Personnel p on pg.PersonnelID = p.PersonnelID where pg.PersonnelGroupID = " & groupID & " order by p.LastName asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Remove_Member(ByVal persID As Integer, ByVal groupID As Integer) As Boolean
        Dim bRemoved As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_Personnel2Group where personnelid = " & persID & " and PersonnelgroupID = " & groupID
            cm.ExecuteNonQuery()
            cm.CommandText = "Select ItemID from t_SecurityItem2Group where groupID = " & groupID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Dim oSecItem2User As New clsSecurityItem2User
                Do While dread.Read
                    If Not (oSecItem2User.Remove_ItemBy_Group(dread("itemID"), persID, groupID)) Then
                        bRemoved = False
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function

    Public Function Add_Member(ByVal persID As Integer, ByVal groupID As Integer) As Boolean
        Dim bAdded As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Insert Into t_Personnel2Group (PersonnelID, PersonnelGroupID) Values (" & persID & "," & groupID & ")"
            cm.ExecuteNonQuery()
            cm.CommandText = "Select ItemID from t_SecurityItem2Group where groupID = " & groupID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Dim oSecItem2User As New clsSecurityItem2User
                Do While dread.Read
                    If Not (oSecItem2User.Item_Exists(dread("ItemID"), persID)) Then
                        If Not (oSecItem2User.Add_Item(dread("itemID"), persID)) Then
                            bAdded = False
                        End If
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bAdded
    End Function

    Public Function Get_Pers_Groups(ByVal persID As Integer) As String
        Dim groups As String = ""
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select PersonnelGroupID as GroupID from t_Personnel2Group where personnelid = " & persID
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Do While dread.Read
                If groups = "" Then
                    groups = dread("GroupID")
                Else
                    groups = groups & dread("GroupID")
                End If
            Loop
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return groups
    End Function

    Public Function List_Pers_Groups(ByVal persID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select PersonnelGroupID as ID, GroupName from t_PersonnelGroup where PersonnelGroupID not in (Select PersonnelGroupID from t_Personnel2Group where personnelid = " & persID & ") order by GroupName asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Available_Members(ByVal groupID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select PersonnelID, LastName + ', ' + FirstName as Personnel from t_Personnel where personnelid not in (Select PersonnelID from t_Personnel2Group where PersonnelGroupID = " & groupID & ") order by Lastname asc, Firstname asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property PersonnelGroupID() As Integer
        Get
            Return _PersonnelGroupID
        End Get
        Set(ByVal value As Integer)
            _PersonnelGroupID = value
        End Set
    End Property

    Public Property PersonnelID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
