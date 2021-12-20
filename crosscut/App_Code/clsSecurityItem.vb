Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsSecurityItem
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _GroupID As Integer = 0
    Dim _Item As String = ""
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
        cm = New SqlCommand("Select * from t_SecurityItem where ItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_SecurityItem where ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_SecurityItem")
            If ds.Tables("t_SecurityItem").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityItem").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("GroupID") Is System.DBNull.Value) Then _GroupID = dr("GroupID")
        If Not (dr("Item") Is System.DBNull.Value) Then _Item = dr("Item")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SecurityItem where ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_SecurityItem")
            If ds.Tables("t_SecurityItem").Rows.Count > 0 Then
                dr = ds.Tables("t_SecurityItem").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_SecurityItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@GroupID", SqlDbType.int, 0, "GroupID")
                da.InsertCommand.Parameters.Add("@Item", SqlDbType.varchar, 0, "Item")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.text, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ItemID", SqlDbType.Int, 0, "ItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_SecurityItem").NewRow
            End If
            Update_Field("GroupID", _GroupID, dr)
            Update_Field("Item", _Item, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_SecurityItem").Rows.Count < 1 Then ds.Tables("t_SecurityItem").Rows.Add(dr)
            da.Update(ds, "t_SecurityItem")
            _ID = ds.Tables("t_SecurityItem").Rows(0).Item("ItemID")
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

    Public Function List_Items_By_Group(ByVal groupID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ItemID as ID, Item from t_SecurityItem where groupid = " & groupID & " order by item asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Item_Exists(ByVal name As String, ByVal GroupID As Integer) As Boolean
        Dim bExists As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_SecurityItem where item = '" & name & "' and GroupID = " & GroupID
            dread = cm.ExecuteReader
            bExists = dread.HasRows()
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bExists = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bExists
    End Function

    Public Property GroupID() As Integer
        Get
            Return _GroupID
        End Get
        Set(ByVal value As Integer)
            _GroupID = value
        End Set
    End Property

    Public Property Item() As String
        Get
            Return _Item
        End Get
        Set(ByVal value As String)
            _Item = value
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

    Public Property ItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
