Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSecurityItem2Group
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _GroupID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SecurityItem2Group where ItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SecurityItem2Group where ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SecurityItem2Group")
            If ds.Tables("t_SecurityItem2Group").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityItem2Group").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("GroupID") Is System.DBNull.Value) Then _GroupID = dr("GroupID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SecurityItem2Group where ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SecurityItem2Group")
            If ds.Tables("t_SecurityItem2Group").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityItem2Group").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SecurityItem2GroupInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@GroupID", SqlDbType.int, 0, "GroupID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ItemID", SqlDbType.Int, 0, "ItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SecurityItem2Group").NewRow
            End If
            Update_Field("GroupID", _GroupID, dr)
            If ds.Tables("t_SecurityItem2Group").Rows.Count < 1 Then ds.Tables("t_SecurityItem2Group").Rows.Add(dr)
            da.Update(ds, "t_SecurityItem2Group")
            _ID = ds.Tables("t_SecurityItem2Group").Rows(0).Item("ItemID")
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
            oEvents.KeyField = "ItemID"
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

    Public Function Check_Item2Group(ByVal itemID As Integer, ByVal groupID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as items from t_SecurityItem2Group where ItemID = " & itemID & " and GroupID = " & GroupID
            dread = cm.ExecuteReader
            dread.Read()
            If dread("items") = 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Function Remove_Item(ByVal itemID As Integer, ByVal groupID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Delete from t_SecurityItem2Group where itemid = " & itemID & " and GroupID = " & groupID
        cm.ExecuteNonQuery()
        cm.CommandText = "Select PersonnelID from t_Personnel2Group where PersonnelgroupID = " & groupID
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Dim oSecItem2User As New clsSecurityItem2User
            Do While dread.Read
                If Not (oSecItem2User.Remove_ItemBy_Group(itemID, dread("PersonnelID"), groupID)) Then
                    bRemoved = False
                End If
            Loop
        End If
        dread.Close()
        'Catch ex As Exception
        '_Err = ex.Message
        'bRemoved = False
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bRemoved
    End Function

    Public Function Add_Item(ByVal itemID As Integer, ByVal groupID As Integer) As Boolean
        Dim bAdded As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Insert Into t_SecurityItem2Group(ItemID, GroupID) Values (" & itemID & "," & groupID & ")"
            cm.ExecuteNonQuery()
            cm.CommandText = "Select PersonnelID from t_Personnel2Group where PersonnelgroupID = " & groupID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Dim oSecItem2User As New clsSecurityItem2User
                Do While dread.Read
                    If Not (oSecItem2User.Item_Exists(itemID, dread("PersonnelID"))) Then
                        If Not (oSecItem2User.Add_Item(itemID, dread("PersonnelID"))) Then
                            bAdded = False
                        End If
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bAdded = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bAdded
    End Function


    Public Property GroupID() As Integer
        Get
            Return _GroupID
        End Get
        Set(ByVal value As Integer)
            _GroupID = value
        End Set
    End Property

    Public Property ItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
