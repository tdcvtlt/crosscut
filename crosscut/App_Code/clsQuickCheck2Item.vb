Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsQuickCheck2Item
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _QuickCheckID As Integer = 0
    Dim _QuickCheckItemID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_QuickCheck2Item where QuickCheck2ItemID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_QuickCheck2Item where QuickCheck2ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_QuickCheck2Item")
            If ds.Tables("t_QuickCheck2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_QuickCheck2Item").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List_Quick_Check_Items(ByVal QuickCheckID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select QuickCheckItemID as ID, i.Description, a.Comboitem as Area ,i.Active from t_QuickCheckItem i left outer join t_Comboitems a on a.comboitemid=i.areaid where quickcheckItemid in (select quickcheckitemid from t_QuickCheck2Item where QuickCheckID=" & QuickCheckID & ") order by description")
    End Function

    Public Sub Remove_Item(ByVal QuickCheckItemID As Integer, ByVal QuickCheckID As Integer)
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "delete from t_QuickCheck2Item where Quickcheckid=" & QuickCheckID & " and QuickCheckItemID = " & QuickCheckItemID
        cm.ExecuteNonQuery()
        If cn.State <> ConnectionState.Closed Then cn.Close()
    End Sub
    Private Sub Set_Values()
        If Not (dr("QuickCheck2ItemID") Is System.DBNull.Value) Then _ID = dr("QuickCheck2ItemID")
        If Not (dr("QuickCheckID") Is System.DBNull.Value) Then _QuickCheckID = dr("QuickCheckID")
        If Not (dr("QuickCheckItemID") Is System.DBNull.Value) Then _QuickCheckItemID = dr("QuickCheckItemID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If _ID = 0 Then
                'Check for duplicate before saving
                Dim dup As Boolean = False
                cm.CommandText = "select * from t_QuickCheck2Item where QuickCheckID=" & _QuickCheckID & " and quickcheckitemid= " & _QuickCheckItemID
                da = New SqlDataAdapter(cm)
                ds = New DataSet
                da.Fill(ds, "Test")
                If ds.Tables("Test").Rows.Count > 0 Then dup = True
                ds = Nothing
                da = Nothing
                If dup Then
                    Return False
                End If
            End If
            cm.CommandText = "Select * from t_QuickCheck2Item where QuickCheck2ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_QuickCheck2Item")
            If ds.Tables("t_QuickCheck2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_QuickCheck2Item").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_QuickCheck2ItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@QuickCheckID", SqlDbType.Int, 0, "QuickCheckID")
                da.InsertCommand.Parameters.Add("@QuickCheckItemID", SqlDbType.Int, 0, "QuickCheckItemID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@QuickCheck2ItemID", SqlDbType.Int, 0, "QuickCheck2ItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_QuickCheck2Item").NewRow
            End If
            Update_Field("QuickCheckID", _QuickCheckID, dr)
            Update_Field("QuickCheckItemID", _QuickCheckItemID, dr)
            If ds.Tables("t_QuickCheck2Item").Rows.Count < 1 Then ds.Tables("t_QuickCheck2Item").Rows.Add(dr)
            da.Update(ds, "t_QuickCheck2Item")
            _ID = ds.Tables("t_QuickCheck2Item").Rows(0).Item("QuickCheck2ItemID")
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
            oEvents.KeyField = "QuickCheck2ItemID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property QuickCheckID() As Integer
        Get
            Return _QuickCheckID
        End Get
        Set(ByVal value As Integer)
            _QuickCheckID = value
        End Set
    End Property

    Public Property QuickCheckItemID() As Integer
        Get
            Return _QuickCheckItemID
        End Get
        Set(ByVal value As Integer)
            _QuickCheckItemID = value
        End Set
    End Property

    Public Property QuickCheck2ItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property ErrorString As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
