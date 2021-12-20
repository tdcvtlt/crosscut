Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProject2Item
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProjectID As Integer = 0
    Dim _ItemNumber As String = ""
    Dim _Qty As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Project2Item where Project2ItemID = " & _ID, cn)
    End Sub

    Public Function List(ProjectID As Integer) As DataTable
        Dim ret As New DataTable
        cm.CommandText = "Select * from t_Project2Item where projectid = '" & ProjectID & "'"
        ret.Load(cm.ExecuteReader)
        Return ret
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Project2Item where Project2ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Project2Item")
            If ds.Tables("t_Project2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_Project2Item").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Delete() As Boolean
        Dim ret As Boolean = False
        If _ID > 0 Then
            cm.CommandText = "delete from t_Project2Item where Project2ItemID='" & _ID & "'"
            cm.ExecuteNonQuery()
            ret = True
        End If
        Return ret
    End Function

    Private Sub Set_Values()
        If Not (dr("Project2ItemID") Is System.DBNull.Value) Then _ID = dr("Project2ItemID")
        If Not (dr("ProjectID") Is System.DBNull.Value) Then _ProjectID = dr("ProjectID")
        If Not (dr("ItemNumber") Is System.DBNull.Value) Then _ItemNumber = dr("ItemNumber")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Project2Item where Project2ItemID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Project2Item")
            If ds.Tables("t_Project2Item").Rows.Count > 0 Then
                dr = ds.Tables("t_Project2Item").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Project2ItemInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProjectID", SqlDbType.Int, 0, "ProjectID")
                da.InsertCommand.Parameters.Add("@ItemNumber", SqlDbType.VarChar, 0, "ItemNumber")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.Int, 0, "Qty")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Project2ItemID", SqlDbType.Int, 0, "Project2ItemID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Project2Item").NewRow
            End If
            Update_Field("ProjectID", _ProjectID, dr)
            Update_Field("ItemNumber", _ItemNumber, dr)
            Update_Field("Qty", _Qty, dr)
            If ds.Tables("t_Project2Item").Rows.Count < 1 Then ds.Tables("t_Project2Item").Rows.Add(dr)
            da.Update(ds, "t_Project2Item")
            _ID = ds.Tables("t_Project2Item").Rows(0).Item("Project2ItemID")
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
            oEvents.KeyField = "Project2ItemID"
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

    Public Property ProjectID() As Integer
        Get
            Return _ProjectID
        End Get
        Set(ByVal value As Integer)
            _ProjectID = value
        End Set
    End Property

    Public Property ItemNumber() As String
        Get
            Return _ItemNumber
        End Get
        Set(ByVal value As String)
            _ItemNumber = value
        End Set
    End Property

    Public Property Qty() As Integer
        Get
            Return _Qty
        End Get
        Set(ByVal value As Integer)
            _Qty = value
        End Set
    End Property

    Public Property Project2ItemID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
