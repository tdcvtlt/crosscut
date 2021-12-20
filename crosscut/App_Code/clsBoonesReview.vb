Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsBoonesReview
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _DateReviewed As String = ""
    Dim _Toppings As Integer = 0
    Dim _Crust As Integer = 0
    Dim _Sauce As Integer = 0
    Dim _Cheese As Integer = 0
    Dim _Presentation As Integer = 0
    Dim _Comments As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_BoonesReview where ReviewID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_BoonesReview where ReviewID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_BoonesReview")
            If ds.Tables("t_BoonesReview").Rows.Count > 0 Then
                dr = ds.Tables("t_BoonesReview").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("DateReviewed") Is System.DBNull.Value) Then _DateReviewed = dr("DateReviewed")
        If Not (dr("Toppings") Is System.DBNull.Value) Then _Toppings = dr("Toppings")
        If Not (dr("Crust") Is System.DBNull.Value) Then _Crust = dr("Crust")
        If Not (dr("Sauce") Is System.DBNull.Value) Then _Sauce = dr("Sauce")
        If Not (dr("Cheese") Is System.DBNull.Value) Then _Cheese = dr("Cheese")
        If Not (dr("Presentation") Is System.DBNull.Value) Then _Presentation = dr("Presentation")
        If Not (dr("Comments") Is System.DBNull.Value) Then _Comments = dr("Comments")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_BoonesReview where ReviewID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_BoonesReview")
            If ds.Tables("t_BoonesReview").Rows.Count > 0 Then
                dr = ds.Tables("t_BoonesReview").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_BoonesReviewInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@DateReviewed", SqlDbType.datetime, 0, "DateReviewed")
                da.InsertCommand.Parameters.Add("@Toppings", SqlDbType.int, 0, "Toppings")
                da.InsertCommand.Parameters.Add("@Crust", SqlDbType.int, 0, "Crust")
                da.InsertCommand.Parameters.Add("@Sauce", SqlDbType.int, 0, "Sauce")
                da.InsertCommand.Parameters.Add("@Cheese", SqlDbType.int, 0, "Cheese")
                da.InsertCommand.Parameters.Add("@Presentation", SqlDbType.int, 0, "Presentation")
                da.InsertCommand.Parameters.Add("@Comments", SqlDbType.ntext, 0, "Comments")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReviewID", SqlDbType.Int, 0, "ReviewID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_BoonesReview").NewRow
            End If
            Update_Field("DateReviewed", _DateReviewed, dr)
            Update_Field("Toppings", _Toppings, dr)
            Update_Field("Crust", _Crust, dr)
            Update_Field("Sauce", _Sauce, dr)
            Update_Field("Cheese", _Cheese, dr)
            Update_Field("Presentation", _Presentation, dr)
            Update_Field("Comments", _Comments, dr)
            If ds.Tables("t_BoonesReview").Rows.Count < 1 Then ds.Tables("t_BoonesReview").Rows.Add(dr)
            da.Update(ds, "t_BoonesReview")
            _ID = ds.Tables("t_BoonesReview").Rows(0).Item("ReviewID")
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
            oEvents.KeyField = "ReviewID"
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

    Public Property DateReviewed() As String
        Get
            Return _DateReviewed
        End Get
        Set(ByVal value As String)
            _DateReviewed = value
        End Set
    End Property

    Public Property Toppings() As Integer
        Get
            Return _Toppings
        End Get
        Set(ByVal value As Integer)
            _Toppings = value
        End Set
    End Property

    Public Property Crust() As Integer
        Get
            Return _Crust
        End Get
        Set(ByVal value As Integer)
            _Crust = value
        End Set
    End Property

    Public Property Sauce() As Integer
        Get
            Return _Sauce
        End Get
        Set(ByVal value As Integer)
            _Sauce = value
        End Set
    End Property

    Public Property Cheese() As Integer
        Get
            Return _Cheese
        End Get
        Set(ByVal value As Integer)
            _Cheese = value
        End Set
    End Property

    Public Property Presentation() As Integer
        Get
            Return _Presentation
        End Get
        Set(ByVal value As Integer)
            _Presentation = value
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

    Public Property ReviewID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

