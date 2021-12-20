Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsFunding
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _DateClosed As String = ""
    Dim _ClosedByID As Integer = 0
    Dim _SubmitToPayroll As Boolean = False
    Dim _ExitFunding As Boolean = False
    Dim _CRMSID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Funding where FundingID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Funding where FundingID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Funding")
            If ds.Tables("t_Funding").Rows.Count > 0 Then
                dr = ds.Tables("t_Funding").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("DateClosed") Is System.DBNull.Value) Then _DateClosed = dr("DateClosed")
        If Not (dr("ClosedByID") Is System.DBNull.Value) Then _ClosedByID = dr("ClosedByID")
        If Not (dr("SubmitToPayroll") Is System.DBNull.Value) Then _SubmitToPayroll = dr("SubmitToPayroll")
        If Not (dr("ExitFunding") Is System.DBNull.Value) Then _ExitFunding = dr("ExitFunding")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Funding where FundingID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Funding")
            If ds.Tables("t_Funding").Rows.Count > 0 Then
                dr = ds.Tables("t_Funding").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_FundingInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@DateClosed", SqlDbType.datetime, 0, "DateClosed")
                da.InsertCommand.Parameters.Add("@ClosedByID", SqlDbType.Int, 0, "ClosedByID")
                da.InsertCommand.Parameters.Add("@SubmitToPayroll", SqlDbType.bit, 0, "SubmitToPayroll")
                da.InsertCommand.Parameters.Add("@ExitFunding", SqlDbType.bit, 0, "ExitFunding")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FundingID", SqlDbType.Int, 0, "FundingID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Funding").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("DateClosed", _DateClosed, dr)
            Update_Field("ClosedByID", _ClosedByID, dr)
            Update_Field("SubmitToPayroll", _SubmitToPayroll, dr)
            Update_Field("ExitFunding", _ExitFunding, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_Funding").Rows.Count < 1 Then ds.Tables("t_Funding").Rows.Add(dr)
            da.Update(ds, "t_Funding")
            _ID = ds.Tables("t_Funding").Rows(0).Item("FundingID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "FundingID"
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

    Public Function List(ByVal exitFunding As Integer, Optional ByVal filter As String = "") As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If filter = "" Then
                ds.SelectCommand = "Select FundingID, Name from t_Funding where exitfunding = '" & exitFunding & "' order by Name desc "
            Else
                ds.SelectCommand = "Select FundingID, Name from t_Funding where exitfunding = '" & exitFunding & "' and Name like '" & filter & "%'"
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Submit_To_Payroll(ByVal fundingID As Integer) As Boolean
        Dim bSuccess As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Update t_Funding set SubmitToPayroll = 1, DateClosed = '" & System.DateTime.Now & "', ClosedByID = '" & _UserID & "' where fundingID = '" & fundingID & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            bSuccess = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bSuccess
    End Function

    Public Function Validate(ByVal fundingName As String) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Fundings from t_Funding where Name = '" & fundingName & "'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Fundings") > 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            bValid = False
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return bValid
    End Function
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property DateClosed() As String
        Get
            Return _DateClosed
        End Get
        Set(ByVal value As String)
            _DateClosed = value
        End Set
    End Property

    Public Property ClosedByID() As Integer
        Get
            Return _ClosedByID
        End Get
        Set(ByVal value As Integer)
            _ClosedByID = value
        End Set
    End Property

    Public Property SubmitToPayroll() As Boolean
        Get
            Return _SubmitToPayroll
        End Get
        Set(ByVal value As Boolean)
            _SubmitToPayroll = value
        End Set
    End Property

    Public Property ExitFunding() As Boolean
        Get
            Return _ExitFunding
        End Get
        Set(ByVal value As Boolean)
            _ExitFunding = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
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

    Public Property FundingID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public ReadOnly Property Err() As String
        Get
            Return _Err
        End Get
    End Property
End Class
