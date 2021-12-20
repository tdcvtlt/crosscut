Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageReservationPersonnel
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PackageReservationID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _TitleID As Integer = 0
    Dim _CommissionPercentage As Decimal = 0
    Dim _FixedAmount As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageReservationPersonnel where PackageReservationPersonnelID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageReservationPersonnel where PackageReservationPersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservationPersonnel")
            If ds.Tables("t_PackageReservationPersonnel").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservationPersonnel").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("PackageReservationID") Is System.DBNull.Value) Then _PackageReservationID = dr("PackageReservationID")
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("TitleID") Is System.DBNull.Value) Then _TitleID = dr("TitleID")
        If Not (dr("CommissionPercentage") Is System.DBNull.Value) Then _CommissionPercentage = dr("CommissionPercentage")
        If Not (dr("FixedAmount") Is System.DBNull.Value) Then _FixedAmount = dr("FixedAmount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageReservationPersonnel where PackageReservationPersonnelID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageReservationPersonnel")
            If ds.Tables("t_PackageReservationPersonnel").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageReservationPersonnel").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageReservationPersonnelInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PackageReservationID", SqlDbType.int, 0, "PackageReservationID")
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@TitleID", SqlDbType.int, 0, "TitleID")
                da.InsertCommand.Parameters.Add("@CommissionPercentage", SqlDbType.float, 0, "CommissionPercentage")
                da.InsertCommand.Parameters.Add("@FixedAmount", SqlDbType.float, 0, "FixedAmount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageReservationPersonnelID", SqlDbType.Int, 0, "PackageReservationPersonnelID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageReservationPersonnel").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PackageReservationID", _PackageReservationID, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("TitleID", _TitleID, dr)
            Update_Field("CommissionPercentage", _CommissionPercentage, dr)
            Update_Field("FixedAmount", _FixedAmount, dr)
            If ds.Tables("t_PackageReservationPersonnel").Rows.Count < 1 Then ds.Tables("t_PackageReservationPersonnel").Rows.Add(dr)
            da.Update(ds, "t_PackageReservationPersonnel")
            _ID = ds.Tables("t_PackageReservationPersonnel").Rows(0).Item("PackageReservationPersonnelID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "PackageReservationPersonnelID"
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

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property PackageReservationID() As Integer
        Get
            Return _PackageReservationID
        End Get
        Set(ByVal value As Integer)
            _PackageReservationID = value
        End Set
    End Property

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property TitleID() As Integer
        Get
            Return _TitleID
        End Get
        Set(ByVal value As Integer)
            _TitleID = value
        End Set
    End Property

    Public Property CommissionPercentage() As Decimal
        Get
            Return _CommissionPercentage
        End Get
        Set(ByVal value As Decimal)
            _CommissionPercentage = value
        End Set
    End Property

    Public Property FixedAmount() As Decimal
        Get
            Return _FixedAmount
        End Get
        Set(ByVal value As Decimal)
            _FixedAmount = value
        End Set
    End Property

    Public Property PackageReservationPersonnelID() As Integer
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
