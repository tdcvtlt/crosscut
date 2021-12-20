Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsOwnerFAQ
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Question As String = ""
    Dim _Answer As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_OwnerFAQ where FAQID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_OwnerFAQ where FAQID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_OwnerFAQ")
            If ds.Tables("t_OwnerFAQ").Rows.Count > 0 Then
                dr = ds.Tables("t_OwnerFAQ").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Question") Is System.DBNull.Value) Then _Question = dr("Question")
        If Not (dr("Answer") Is System.DBNull.Value) Then _Answer = dr("Answer")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_OwnerFAQ where FAQID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_OwnerFAQ")
            If ds.Tables("t_OwnerFAQ").Rows.Count > 0 Then
                dr = ds.Tables("t_OwnerFAQ").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_OwnerFAQInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Question", SqlDbType.text, 0, "Question")
                da.InsertCommand.Parameters.Add("@Answer", SqlDbType.text, 0, "Answer")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@FAQID", SqlDbType.Int, 0, "FAQID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_OwnerFAQ").NewRow
            End If
            Update_Field("Question", _Question, dr)
            Update_Field("Answer", _Answer, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_OwnerFAQ").Rows.Count < 1 Then ds.Tables("t_OwnerFAQ").Rows.Add(dr)
            da.Update(ds, "t_OwnerFAQ")
            _ID = ds.Tables("t_OwnerFAQ").Rows(0).Item("FAQID")
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
            oEvents.KeyField = "FAQID"
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

    Public Property Question() As String
        Get
            Return _Question
        End Get
        Set(ByVal value As String)
            _Question = value
        End Set
    End Property

    Public Property Answer() As String
        Get
            Return _Answer
        End Get
        Set(ByVal value As String)
            _Answer = value
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

    Public Property FAQID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

