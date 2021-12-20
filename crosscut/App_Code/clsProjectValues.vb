Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProjectValues
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProjectID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _AreaID As Integer = 0
    Dim _Value As Integer = 0
    Dim _DateUpdated As String = ""    
    Dim _Comments As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ProjectValues where ProjectStatusID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ProjectValues where ProjectStatusID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ProjectValues")
            If ds.Tables("t_ProjectValues").Rows.Count > 0 Then
                dr = ds.Tables("t_ProjectValues").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Sub Load_Details(ProjectID As Integer, RoomID As Integer, AreaID As Integer)
        Try
            cm.CommandText = "Select * from t_ProjectValues where ProjectID = " & ProjectID & " and RoomID = " & RoomID & " and AreaID = " & AreaID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ProjectValues")
            If ds.Tables("t_ProjectValues").Rows.Count > 0 Then
                dr = ds.Tables("t_ProjectValues").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ProjectStatusID") Is System.DBNull.Value) Then _ID = dr("ProjectStatusID")
        If Not (dr("ProjectID") Is System.DBNull.Value) Then _ProjectID = dr("ProjectID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
        If Not (dr("AreaID") Is System.DBNull.Value) Then _AreaID = dr("AreaID")
        If Not (dr("Value") Is System.DBNull.Value) Then _Value = dr("Value")
        If Not (dr("DateUpdated") Is System.DBNull.Value) Then _DateUpdated = dr("DateUpdated")
        If Not (dr("UserID") Is System.DBNull.Value) Then _UserID = dr("UserID")
        If Not (dr("Comments") Is System.DBNull.Value) Then _Comments = dr("Comments")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProjectValues where ProjectStatusID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ProjectValues")
            If ds.Tables("t_ProjectValues").Rows.Count > 0 Then
                dr = ds.Tables("t_ProjectValues").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ProjectValuesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProjectID", SqlDbType.int, 0, "ProjectID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                da.InsertCommand.Parameters.Add("@AreaID", SqlDbType.int, 0, "AreaID")
                da.InsertCommand.Parameters.Add("@Value", SqlDbType.int, 0, "Value")
                da.InsertCommand.Parameters.Add("@DateUpdated", SqlDbType.datetime, 0, "DateUpdated")
                da.InsertCommand.Parameters.Add("@UserID", SqlDbType.int, 0, "UserID")
                da.InsertCommand.Parameters.Add("@Comments", SqlDbType.text, 0, "Comments")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ProjectStatusID", SqlDbType.Int, 0, "ProjectStatusID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ProjectValues").NewRow
            End If
            Update_Field("ProjectID", _ProjectID, dr)
            Update_Field("RoomID", _RoomID, dr)
            Update_Field("AreaID", _AreaID, dr)
            Update_Field("Value", _Value, dr)
            Update_Field("DateUpdated", _DateUpdated, dr)
            Update_Field("UserID", _UserID, dr)
            Update_Field("Comments", _Comments, dr)
            If ds.Tables("t_ProjectValues").Rows.Count < 1 Then ds.Tables("t_ProjectValues").Rows.Add(dr)
            da.Update(ds, "t_ProjectValues")
            _ID = ds.Tables("t_ProjectValues").Rows(0).Item("ProjectStatusID")
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
            oEvents.KeyField = "ProjectStatusID"
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

    Public Property ProjectID() As Integer
        Get
            Return _ProjectID
        End Get
        Set(ByVal value As Integer)
            _ProjectID = value
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

    Public Property AreaID() As Integer
        Get
            Return _AreaID
        End Get
        Set(ByVal value As Integer)
            _AreaID = value
        End Set
    End Property

    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            _Value = value
        End Set
    End Property

    Public Property DateUpdated() As String
        Get
            Return _DateUpdated
        End Get
        Set(ByVal value As String)
            _DateUpdated = value
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

    Public Property Comments() As String
        Get
            Return _Comments
        End Get
        Set(ByVal value As String)
            _Comments = value
        End Set
    End Property

    Public Property ProjectStatusID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
End Class
