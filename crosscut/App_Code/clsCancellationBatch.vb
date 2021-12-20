Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsCancellationBatch
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _HearingDate As String = ""
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _StatusDate As String = ""
    Dim _LienNo As String = ""
    Dim _PublicationDate1 As String = ""
    Dim _PublicationDate2 As String = ""
    Dim _PublicationDate3 As String = ""
    Dim _PublicationDate4 As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_CancellationBatch where BatchID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_CancellationBatch where BatchID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_CancellationBatch")
            If ds.Tables("t_CancellationBatch").Rows.Count > 0 Then
                dr = ds.Tables("t_CancellationBatch").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("HearingDate") Is System.DBNull.Value) Then _HearingDate = dr("HearingDate")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("CreatedByID") Is System.DBNull.Value) Then _CreatedByID = dr("CreatedByID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("LienNo") Is System.DBNull.Value) Then _LienNo = dr("LienNo")
        If Not (dr("PublicationDate1") Is System.DBNull.Value) Then _PublicationDate1 = dr("PublicationDate1")
        If Not (dr("PublicationDate2") Is System.DBNull.Value) Then _PublicationDate2 = dr("PublicationDate2")
        If Not (dr("PublicationDate3") Is System.DBNull.Value) Then _PublicationDate3 = dr("PublicationDate3")
        If Not (dr("PublicationDate4") Is System.DBNull.Value) Then _PublicationDate4 = dr("PublicationDate4")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_CancellationBatch where BatchID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_CancellationBatch")
            If ds.Tables("t_CancellationBatch").Rows.Count > 0 Then
                dr = ds.Tables("t_CancellationBatch").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_CancellationBatchInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@HearingDate", SqlDbType.DateTime, 0, "HearingDate")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.Int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.DateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@LienNo", SqlDbType.VarChar, 0, "LienNo")
                da.InsertCommand.Parameters.Add("@PublicationDate1", SqlDbType.DateTime, 0, "PublicationDate1")
                da.InsertCommand.Parameters.Add("@PublicationDate2", SqlDbType.DateTime, 0, "PublicationDate2")
                da.InsertCommand.Parameters.Add("@PublicationDate3", SqlDbType.DateTime, 0, "PublicationDate3")
                da.InsertCommand.Parameters.Add("@PublicationDate4", SqlDbType.DateTime, 0, "PublicationDate4")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@BatchID", SqlDbType.Int, 0, "BatchID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_CancellationBatch").NewRow
            End If
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("HearingDate", _HearingDate, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("LienNo", _LienNo, dr)
            Update_Field("PublicationDate1", _PublicationDate1, dr)
            Update_Field("PublicationDate2", _PublicationDate2, dr)
            Update_Field("PublicationDate3", _PublicationDate3, dr)
            Update_Field("PublicationDate4", _PublicationDate4, dr)

            If ds.Tables("t_CancellationBatch").Rows.Count < 1 Then ds.Tables("t_CancellationBatch").Rows.Add(dr)
            da.Update(ds, "t_CancellationBatch")
            _ID = ds.Tables("t_CancellationBatch").Rows(0).Item("BatchID")
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
            oEvents.KeyField = "BatchID"
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

    Public Property PublicationDate1 As String
        Get
            Return _PublicationDate1
        End Get
        Set(value As String)
            _PublicationDate1 = value
        End Set
    End Property

    Public Property PublicationDate2 As String
        Get
            Return _PublicationDate2
        End Get
        Set(value As String)
            _PublicationDate2 = value
        End Set
    End Property

    Public Property PublicationDate3 As String
        Get
            Return _PublicationDate3
        End Get
        Set(value As String)
            _PublicationDate3 = value
        End Set
    End Property

    Public Property PublicationDate4 As String
        Get
            Return _PublicationDate4
        End Get
        Set(value As String)
            _PublicationDate4 = value
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

    Public Property HearingDate() As String
        Get
            Return _HearingDate
        End Get
        Set(ByVal value As String)
            _HearingDate = value
        End Set
    End Property

    Public Property LienNo() As String
        Get
            Return _LienNo
        End Get
        Set(value As String)
            _LienNo = value
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

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
        End Set
    End Property

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property

    Public Property BatchID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
