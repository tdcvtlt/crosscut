Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRegCard2ResType
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RegCardID As Integer = 0
    Dim _ResTypeID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RegCard2ResType where RegCard2TypeID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RegCard2ResType where RegCard2TypeID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RegCard2ResType")
            If ds.Tables("t_RegCard2ResType").Rows.Count > 0 Then
                dr = ds.Tables("t_RegCard2ResType").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RegCardID") Is System.DBNull.Value) Then _RegCardID = dr("RegCardID")
        If Not (dr("ResTypeID") Is System.DBNull.Value) Then _ResTypeID = dr("ResTypeID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RegCard2ResType where RegCard2TypeID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RegCard2ResType")
            If ds.Tables("t_RegCard2ResType").Rows.Count > 0 Then
                dr = ds.Tables("t_RegCard2ResType").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RegCard2ResTypeInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RegCardID", SqlDbType.int, 0, "RegCardID")
                da.InsertCommand.Parameters.Add("@ResTypeID", SqlDbType.int, 0, "ResTypeID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RegCard2TypeID", SqlDbType.Int, 0, "RegCard2TypeID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RegCard2ResType").NewRow
            End If
            Update_Field("RegCardID", _RegCardID, dr)
            Update_Field("ResTypeID", _ResTypeID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_RegCard2ResType").Rows.Count < 1 Then ds.Tables("t_RegCard2ResType").Rows.Add(dr)
            da.Update(ds, "t_RegCard2ResType")
            _ID = ds.Tables("t_RegCard2ResType").Rows(0).Item("RegCard2TypeID")
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
            oEvents.KeyField = "RegCard2TypeID"
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

    Public Function List_RegCard_Types(ByVal regID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select RegCard2TypeID as ID, rt.Comboitem as ResType, r.Active from t_RegCard2ResType r inner join t_ComboItems rt on r.ResTypeID = rt.ComboItemID where r.RegCardID = " & regID
        Return ds
    End Function

    Public Property RegCardID() As Integer
        Get
            Return _RegCardID
        End Get
        Set(ByVal value As Integer)
            _RegCardID = value
        End Set
    End Property

    Public Property ResTypeID() As Integer
        Get
            Return _ResTypeID
        End Get
        Set(ByVal value As Integer)
            _ResTypeID = value
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

    Public Property RegCard2TypeID() As Integer
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

End Class
