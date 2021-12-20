Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLeadFileItems
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _FileID As Integer = 0
    Dim _Row_Text As String = ""
    Dim _Number As String = ""
    Dim _HeaderRow As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LeadFileItems where RowID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LeadFileItems where RowID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LeadFileItems")
            If ds.Tables("t_LeadFileItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadFileItems").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("FileID") Is System.DBNull.Value) Then _FileID = dr("FileID")
        If Not (dr("Row_Text") Is System.DBNull.Value) Then _Row_Text = dr("Row_Text")
        If Not (dr("Number") Is System.DBNull.Value) Then _Number = dr("Number")
        If Not (dr("HeaderRow") Is System.DBNull.Value) Then _HeaderRow = dr("HeaderRow")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LeadFileItems where RowID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LeadFileItems")
            If ds.Tables("t_LeadFileItems").Rows.Count > 0 Then
                dr = ds.Tables("t_LeadFileItems").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LeadFileItemsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@FileID", SqlDbType.int, 0, "FileID")
                da.InsertCommand.Parameters.Add("@Row_Text", SqlDbType.text, 0, "Row_Text")
                da.InsertCommand.Parameters.Add("@Number", SqlDbType.varchar, 0, "Number")
                da.InsertCommand.Parameters.Add("@HeaderRow", SqlDbType.bit, 0, "HeaderRow")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RowID", SqlDbType.Int, 0, "RowID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LeadFileItems").NewRow
            End If
            Update_Field("FileID", _FileID, dr)
            Update_Field("Row_Text", _Row_Text, dr)
            Update_Field("Number", _Number, dr)
            Update_Field("HeaderRow", _HeaderRow, dr)
            If ds.Tables("t_LeadFileItems").Rows.Count < 1 Then ds.Tables("t_LeadFileItems").Rows.Add(dr)
            da.Update(ds, "t_LeadFileItems")
            _ID = ds.Tables("t_LeadFileItems").Rows(0).Item("RowID")
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
            oEvents.KeyField = "RowID"
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

    Public Property FileID() As Integer
        Get
            Return _FileID
        End Get
        Set(ByVal value As Integer)
            _FileID = value
        End Set
    End Property

    Public Property Row_Text() As String
        Get
            Return _Row_Text
        End Get
        Set(ByVal value As String)
            _Row_Text = value
        End Set
    End Property

    Public Property Number() As String
        Get
            Return _Number
        End Get
        Set(ByVal value As String)
            _Number = value
        End Set
    End Property

    Public Property HeaderRow() As Boolean
        Get
            Return _HeaderRow
        End Get
        Set(ByVal value As Boolean)
            _HeaderRow = value
        End Set
    End Property

    Public Property RowID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
