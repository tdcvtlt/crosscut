Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSecurityItem2User
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _GroupID As Integer = 0
    Dim _Allow As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_SecurityItem2User where ItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SecurityItem2User where ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SecurityItem2User")
            If ds.Tables("t_SecurityItem2User").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityItem2User").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("GroupID") Is System.DBNull.Value) Then _GroupID = dr("GroupID")
        If Not (dr("Allow") Is System.DBNull.Value) Then _Allow = dr("Allow")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SecurityItem2User where ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SecurityItem2User")
            If ds.Tables("t_SecurityItem2User").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityItem2User").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SecurityItem2UserInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@GroupID", SqlDbType.int, 0, "GroupID")
                da.InsertCommand.Parameters.Add("@Allow", SqlDbType.bit, 0, "Allow")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ItemID", SqlDbType.Int, 0, "ItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SecurityItem2User").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("GroupID", _GroupID, dr)
            Update_Field("Allow", _Allow, dr)
            If ds.Tables("t_SecurityItem2User").Rows.Count < 1 Then ds.Tables("t_SecurityItem2User").Rows.Add(dr)
            da.Update(ds, "t_SecurityItem2User")
            _ID = ds.Tables("t_SecurityItem2User").Rows(0).Item("ItemID")
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

    Public Function Check_Item2User(ByVal itemID As Integer, ByVal persID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as items from t_SecurityItem2User where ItemID = " & itemID & " and PersonnelID = " & persID
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

    Public Function Remove_Item(ByVal itemID As Integer, ByVal persID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_SecurityItem2User where itemid = " & itemID & " and PersonnelID = " & persID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bRemoved = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function

    Public Function Add_Item(ByVal itemID As Integer, ByVal persID As Integer) As Boolean
        Dim bAdded As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Insert Into t_SecurityItem2User(ItemID, PersonnelID, GroupID, Allow) Values (" & itemID & "," & persID & ",0,0)"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bAdded = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bAdded
    End Function

    Public Function Remove_ItemBy_Group(ByVal itemID As Integer, ByVal persID As Integer, ByVal groupID As Integer) As Boolean
        Dim bRemoved As Boolean = True
        'Try
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Delete from t_SecurityItem2User where ItemID = '" & itemID & "' and PersonnelID = '" & persID & "' and itemid not in (Select itemid from t_SecurityItem2Group where itemid = '" & itemID & "' and GroupID in (Select personnelgroupid from t_Personnel2Group where personnelgroupid <> '" & groupID & "' and personnelid = '" & persID & "'))"
        cm.ExecuteNonQuery()
        'Catch ex As Exception
        '_Err = ex.Message
        'bRemoved = False
        'Finally
        If cn.State <> ConnectionState.Closed Then cn.Close()
        'End Try
        Return bRemoved
    End Function

    Public Function Item_Exists(ByVal itemID As Integer, ByVal persID As Integer) As Boolean
        Dim bExists As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SecurityItem2User where ItemID = " & itemID & " and personnelID = " & persID
            dread = cm.ExecuteReader
            bExists = dread.HasRows
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bExists
    End Function

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property GroupID() As Integer
        Get
            Return _GroupID
        End Get
        Set(ByVal value As Integer)
            _GroupID = value
        End Set
    End Property

    Public Property Allow() As Boolean
        Get
            Return _Allow
        End Get
        Set(ByVal value As Boolean)
            _Allow = value
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
