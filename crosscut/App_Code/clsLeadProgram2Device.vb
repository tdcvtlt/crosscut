Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadProgram2Device
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LeadProgramID As Integer = 0
    Dim _Device As String = ""
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
        cm = New SqlCommand("Select * from t_LeadProgram2Device where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadProgram2Device where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram2Device")
            If ds.Tables("t_LeadProgram2Device").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram2Device").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LeadProgramID") Is System.DBNull.Value) Then _LeadProgramID = dr("LeadProgramID")
        If Not (dr("Device") Is System.DBNull.Value) Then _Device = dr("Device")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadProgram2Device where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadProgram2Device")
            If ds.Tables("t_LeadProgram2Device").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadProgram2Device").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadProgram2DeviceInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LeadProgramID", SqlDbType.int, 0, "LeadProgramID")
                da.InsertCommand.Parameters.Add("@Device", SqlDbType.varchar, 0, "Device")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadProgram2Device").NewRow
            End If
            Update_Field("LeadProgramID", _LeadProgramID, dr)
            Update_Field("Device", _Device, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_LeadProgram2Device").Rows.Count < 1 Then ds.Tables("t_LeadProgram2Device").Rows.Add(dr)
            da.Update(ds, "t_LeadProgram2Device")
            _ID = ds.Tables("t_LeadProgram2Device").Rows(0).Item("ID")
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
            oEvents.KeyField = "ID"
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

    Public Function get_Program_ID(ByVal device As String) As Integer
        Dim ID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select LeadProgramID from t_LeadProgram2Device where device = '" & device & "' and active = 1"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ID = dread("LeadProgramID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ID
    End Function

    Public Function List_Devices(ByVal LPID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ID, Device, Active from t_LeadProgram2Device where LeadProgramID = " & LPID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property LeadProgramID() As Integer
        Get
            Return _LeadProgramID
        End Get
        Set(ByVal value As Integer)
            _LeadProgramID = value
        End Set
    End Property

    Public Property Device() As String
        Get
            Return _Device
        End Get
        Set(ByVal value As String)
            _Device = value
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

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
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


    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
