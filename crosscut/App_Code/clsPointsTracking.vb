Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPointsTracking
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _TransTypeID As Integer = 0
    Dim _Points As Integer = 0
    Dim _AvailYear As Integer = 0
    Dim _UsageYear As Integer = 0
    Dim _ExpirationDate As String = ""
    Dim _StayLocID As Integer = 0
    Dim _CreatedByID As Integer = 0
    Dim _TransDate As String = ""
    Dim _Comments As String = ""
    Dim _ApplyToID As Integer = 0
    Dim _PosNeg As Boolean = False
    Dim _ProspectID As Integer = 0
    Dim _ConfirmationNumber As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PointsTracking where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PointsTracking where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PointsTracking")
            If ds.Tables("t_PointsTracking").Rows.Count > 0 Then
                dr = ds.Tables("t_PointsTracking").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ID") Is System.DBNull.Value) Then _ID = dr("ID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("TransTypeID") Is System.DBNull.Value) Then _TransTypeID = dr("TransTypeID")
        If Not (dr("Points") Is System.DBNull.Value) Then _Points = dr("Points")
        If Not (dr("AvailYear") Is System.DBNull.Value) Then _AvailYear = dr("AvailYear")
        If Not (dr("UsageYear") Is System.DBNull.Value) Then _UsageYear = dr("UsageYear")
        If Not (dr("ExpirationDate") Is System.DBNull.Value) Then _ExpirationDate = dr("ExpirationDate")
        If Not (dr("StayLocID") Is System.DBNull.Value) Then _StayLocID = dr("StayLocID")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("TransDate") Is System.DBNull.Value) Then _TransDate = dr("TransDate")
        If Not (dr("Comments") Is System.DBNull.Value) Then _Comments = dr("Comments")
        If Not (dr("ApplyToID") Is System.DBNull.Value) Then _ApplyToID = dr("ApplyToID")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("ConfirmationNumber") Is System.DBNull.Value) Then _ConfirmationNumber = dr("ConfirmationNumber")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PointsTracking where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PointsTracking")
            If ds.Tables("t_PointsTracking").Rows.Count > 0 Then
                dr = ds.Tables("t_PointsTracking").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PointsTrackingInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@TransTypeID", SqlDbType.int, 0, "TransTypeID")
                da.InsertCommand.Parameters.Add("@Points", SqlDbType.int, 0, "Points")
                da.InsertCommand.Parameters.Add("@AvailYear", SqlDbType.int, 0, "AvailYear")
                da.InsertCommand.Parameters.Add("@UsageYear", SqlDbType.int, 0, "UsageYear")
                da.InsertCommand.Parameters.Add("@ExpirationDate", SqlDbType.datetime, 0, "ExpirationDate")
                da.InsertCommand.Parameters.Add("@StayLocID", SqlDbType.int, 0, "StayLocID")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@TransDate", SqlDbType.datetime, 0, "TransDate")
                da.InsertCommand.Parameters.Add("@Comments", SqlDbType.varchar, 0, "Comments")
                da.InsertCommand.Parameters.Add("@ApplyToID", SqlDbType.int, 0, "ApplyToID")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.Bit, 0, "PosNeg")
                da.InsertCommand.Parameters.Add("@ConfirmationNumber", SqlDbType.VarChar, 0, "ConfirmationNumber")
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Bit, 0, "ProspectID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PointsTracking").NewRow
            End If
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("TransTypeID", _TransTypeID, dr)
            Update_Field("Points", _Points, dr)
            Update_Field("AvailYear", _AvailYear, dr)
            Update_Field("UsageYear", _UsageYear, dr)
            Update_Field("ExpirationDate", _ExpirationDate, dr)
            Update_Field("StayLocID", _StayLocID, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("TransDate", _TransDate, dr)
            Update_Field("Comments", _Comments, dr)
            Update_Field("ApplyToID", _ApplyToID, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ConfirmationNumber", _ConfirmationNumber, dr)
            If ds.Tables("t_PointsTracking").Rows.Count < 1 Then ds.Tables("t_PointsTracking").Rows.Add(dr)
            da.Update(ds, "t_PointsTracking")
            _ID = ds.Tables("t_PointsTracking").Rows(0).Item("ID")
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

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property TransTypeID() As Integer
        Get
            Return _TransTypeID
        End Get
        Set(ByVal value As Integer)
            _TransTypeID = value
        End Set
    End Property

    Public Property Points() As Integer
        Get
            Return _Points
        End Get
        Set(ByVal value As Integer)
            _Points = value
        End Set
    End Property

    Public Property AvailYear() As Integer
        Get
            Return _AvailYear
        End Get
        Set(ByVal value As Integer)
            _AvailYear = value
        End Set
    End Property

    Public Property UsageYear() As Integer
        Get
            Return _UsageYear
        End Get
        Set(ByVal value As Integer)
            _UsageYear = value
        End Set
    End Property

    Public Property ExpirationDate() As String
        Get
            Return _ExpirationDate
        End Get
        Set(ByVal value As String)
            _ExpirationDate = value
        End Set
    End Property

    Public Property StayLocID() As Integer
        Get
            Return _StayLocID
        End Get
        Set(ByVal value As Integer)
            _StayLocID = value
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

    Public Property TransDate() As String
        Get
            Return _TransDate
        End Get
        Set(ByVal value As String)
            _TransDate = value
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

    Public Property ApplyToID() As Integer
        Get
            Return _ApplyToID
        End Get
        Set(ByVal value As Integer)
            _ApplyToID = value
        End Set
    End Property

    Public Property PosNeg() As Boolean
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Boolean)
            _PosNeg = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property


    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property ConfirmationNumber As String
        Get
            Return _ConfirmationNumber
        End Get
        Set(value As String)
            _ConfirmationNumber = value
        End Set
    End Property
End Class
