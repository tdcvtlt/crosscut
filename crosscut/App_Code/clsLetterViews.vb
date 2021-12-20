Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLetterViews
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _View As String = ""
    Dim _Source As Boolean = False
    Dim _KeyField As String = ""
    Dim _TypeID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LetterViews where ViewID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LetterViews where ViewID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LetterViews")
            If ds.Tables("t_LetterViews").Rows.Count > 0 Then
                dr = ds.Tables("t_LetterViews").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(ByVal TypeID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LetterViews where typeid = " & TypeID)
    End Function

    Public Function List_Views() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from sys.views order by Name")
    End Function

    Public Function List_View_Columns(ByVal view As String) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "SELECT * FROM information_schema.columns WHERE table_name = '" & view & "'")
    End Function

    Public Function List_Views_By_Type(ByVal TypeID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from t_LetterViews where Typeid=" & TypeID & " order by [View]")
    End Function

    Public Function List_Source_View_Columns(ByVal TypeID As Integer) As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from information_schema.columns WHERE table_name in (select [View] from t_LetterViews where Typeid = " & TypeID & " and source=1)")
    End Function

    Private Sub Set_Values()
        If Not (dr("View") Is System.DBNull.Value) Then _View = dr("View")
        If Not (dr("Source") Is System.DBNull.Value) Then _Source = dr("Source")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If _Source Then
                cm.CommandText = "Update t_LetterViews set source = 0 where source = 1 and typeid = " & _TypeID
                cm.ExecuteNonQuery()
            End If
            cm.CommandText = "Select * from t_LetterViews where ViewID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LetterViews")
            If ds.Tables("t_LetterViews").Rows.Count > 0 Then
                dr = ds.Tables("t_LetterViews").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LetterViewsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@View", SqlDbType.VarChar, 0, "View")
                da.InsertCommand.Parameters.Add("@Source", SqlDbType.Bit, 0, "Source")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.VarChar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ViewID", SqlDbType.Int, 0, "ViewID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LetterViews").NewRow
            End If
            Update_Field("View", _View, dr)
            Update_Field("Source", _Source, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("TypeID", _TypeID, dr)
            If ds.Tables("t_LetterViews").Rows.Count < 1 Then ds.Tables("t_LetterViews").Rows.Add(dr)
            da.Update(ds, "t_LetterViews")
            _ID = ds.Tables("t_LetterViews").Rows(0).Item("ViewID")
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
            oEvents.KeyField = "ViewID"
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

    Public Property View() As String
        Get
            Return _View
        End Get
        Set(ByVal value As String)
            _View = value
        End Set
    End Property

    Public Property Source() As Boolean
        Get
            Return _Source
        End Get
        Set(ByVal value As Boolean)
            _Source = value
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

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property ViewID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
