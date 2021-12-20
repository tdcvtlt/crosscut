Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsLetterTagStyles
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Style As String = ""
    Dim _StartingTag As String = ""
    Dim _ItemTag As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_LetterTagStyles where TagStyleID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_LetterTagStyles where TagStyleID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_LetterTagStyles")
            If ds.Tables("t_LetterTagStyles").Rows.Count > 0 Then
                dr = ds.Tables("t_LetterTagStyles").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Style") Is System.DBNull.Value) Then _Style = dr("Style")
        If Not (dr("StartingTag") Is System.DBNull.Value) Then _StartingTag = dr("StartingTag")
        If Not (dr("ItemTag") Is System.DBNull.Value) Then _ItemTag = dr("ItemTag")
    End Sub

    Public Function List() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select 0 as TagStyleID, '' as Style union Select TagStyleID, Style from t_LetterTagStyles order by Style")
    End Function

    Public Function List_FilterID(ByVal id As Integer) As SqlDataSource
        If id > 0 Then
            Return New SqlDataSource(Resources.Resource.cns, "Select TagStyleID, Style from t_LetterTagStyles where tagstyleid = '" & id & "' order by Style")
        Else
            Return New SqlDataSource(Resources.Resource.cns, "Select TagStyleID, Style from t_LetterTagStyles order by Style")
        End If
    End Function

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_LetterTagStyles where TagStyleID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_LetterTagStyles")
            If ds.Tables("t_LetterTagStyles").Rows.Count > 0 Then
                dr = ds.Tables("t_LetterTagStyles").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_LetterTagStylesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Style", SqlDbType.VarChar, 0, "Style")
                da.InsertCommand.Parameters.Add("@StartingTag", SqlDbType.VarChar, 0, "StartingTag")
                da.InsertCommand.Parameters.Add("@ItemTag", SqlDbType.VarChar, 0, "ItemTag")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@TagStyleID", SqlDbType.Int, 0, "TagStyleID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_LetterTagStyles").NewRow
            End If
            Update_Field("Style", _Style, dr)
            Update_Field("StartingTag", _StartingTag, dr)
            Update_Field("ItemTag", _ItemTag, dr)
            If ds.Tables("t_LetterTagStyles").Rows.Count < 1 Then ds.Tables("t_LetterTagStyles").Rows.Add(dr)
            da.Update(ds, "t_LetterTagStyles")
            _ID = ds.Tables("t_LetterTagStyles").Rows(0).Item("TagStyleID")
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
            oEvents.KeyField = "TagStyleID"
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

    Public Property Style() As String
        Get
            Return _Style
        End Get
        Set(ByVal value As String)
            _Style = value
        End Set
    End Property

    Public Property StartingTag() As String
        Get
            Return _StartingTag
        End Get
        Set(ByVal value As String)
            _StartingTag = value
        End Set
    End Property

    Public Property ItemTag() As String
        Get
            Return _ItemTag
        End Get
        Set(ByVal value As String)
            _ItemTag = value
        End Set
    End Property

    Public Property TagStyleID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
