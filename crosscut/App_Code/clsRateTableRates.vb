Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsRateTableRates
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _RateTableID As Integer = 0
    Dim _Date As String = ""
    Dim _Amount As Decimal = 0
    Dim _RentalAmount As Decimal = 0
    Dim _Cost As Decimal = 0
    Dim _OwnerAmount As Integer = 0
    Dim _TSAmount As Integer = 0
    Dim _TPAmount As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_RateTableRates where RateTableRateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_RateTableRates where RateTableRateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_RateTableRates")
            If ds.Tables("t_RateTableRates").Rows.Count > 0 Then
                dr = ds.Tables("t_RateTableRates").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("RateTableID") Is System.DBNull.Value) Then _RateTableID = dr("RateTableID")
        If Not (dr("Date") Is System.DBNull.Value) Then _Date = dr("Date")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("RentalAmount") Is System.DBNull.Value) Then _RentalAmount = dr("RentalAmount")
        If Not (dr("Cost") Is System.DBNull.Value) Then _Cost = dr("Cost")
        If Not (dr("OwnerAmount") Is System.DBNull.Value) Then _OwnerAmount = dr("OwnerAmount")
        If Not (dr("TSAmount") Is System.DBNull.Value) Then _TSAmount = dr("TSAmount")
        If Not (dr("TPAmount") Is System.DBNull.Value) Then _TPAmount = dr("TPAmount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_RateTableRates where RateTableRateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_RateTableRates")
            If ds.Tables("t_RateTableRates").Rows.Count > 0 Then
                dr = ds.Tables("t_RateTableRates").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_RateTableRatesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@RateTableID", SqlDbType.int, 0, "RateTableID")
                da.InsertCommand.Parameters.Add("@Date", SqlDbType.datetime, 0, "Date")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.Money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@RentalAmount", SqlDbType.Money, 0, "RentalAmount")
                da.InsertCommand.Parameters.Add("@Cost", SqlDbType.Money, 0, "Cost")
                da.InsertCommand.Parameters.Add("@OwnerAmount", SqlDbType.Money, 0, "OwnerAmount")
                da.InsertCommand.Parameters.Add("@TSAmount", SqlDbType.Money, 0, "TSAmount")
                da.InsertCommand.Parameters.Add("@TPAmount", SqlDbType.Money, 0, "TPAmount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RateTableRateID", SqlDbType.Int, 0, "RateTableRateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_RateTableRates").NewRow
            End If
            Update_Field("RateTableID", _RateTableID, dr)
            Update_Field("Date", _Date, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("RentalAmount", _RentalAmount, dr)
            Update_Field("Cost", _Cost, dr)
            Update_Field("OwnerAmount", _OwnerAmount, dr)
            Update_Field("TSAmount", _TSAmount, dr)
            Update_Field("TPAmount", _TPAmount, dr)
            If ds.Tables("t_RateTableRates").Rows.Count < 1 Then ds.Tables("t_RateTableRates").Rows.Add(dr)
            da.Update(ds, "t_RateTableRates")
            _ID = ds.Tables("t_RateTableRates").Rows(0).Item("RateTableRateID")
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
            oEvents.KeyField = "RateTableRateID"
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

    Public Function Get_Rates(ByVal rateTableID As Integer, ByVal sDate As DateTime, ByVal eDate As DateTime) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select RateTableRateID as ID, Date, Amount, RentalAmount,OwnerAmount,TSAmount as TradeShowAmount,TPAmount as TourPackageAmount from t_RateTableRates where ratetableid = " & rateTableID & " and date between '" & sDate & "' and '" & eDate & "' order by Date asc"
        Return ds
    End Function

    Public Function Find_ID(ByVal tableID As Integer, ByVal sdate As DateTime) As Integer
        Dim tblID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select RateTableRateID from t_RateTableRates where RateTableID = " & tableID & " and Date = '" & sdate & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                tblID = dread("RateTablerateID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return tblID
    End Function

    Public Function Build_Rates(ByVal rateTableID As Integer, ByVal sDate As DateTime, ByVal eDate As DateTime, ByVal wkDayRate As Double, ByVal wkEndRate As Double, ByVal wkDayRental As Double, ByVal wkEndRental As Double, ByVal wkDayCost As Double, ByVal wkEndCost As Double, ByVal wkDayOwner As Double, ByVal wkEndOwner As Double, ByVal wkDayTSRate As Double, ByVal wkEndTSRate As Double, ByVal wkDayTPRate As Double, ByVal wkEndTPRate As Double) As Boolean
        Dim bBuilt As Boolean = False
        Try
            Dim tempDate As DateTime = sDate
            Do While DateTime.Compare(tempDate, eDate) < 1
                Dim oRates As New clsRateTableRates
                oRates.RateTableRateID = Find_ID(rateTableID, tempDate)
                oRates.Load()
                oRates.UserID = _UserID
                oRates.RateTableID = rateTableID
                oRates.RateDate = tempDate
                If tempDate.DayOfWeek = DayOfWeek.Saturday Or tempDate.DayOfWeek = DayOfWeek.Friday Then
                    oRates.Amount = wkEndRate
                    oRates.RentalAmount = wkEndRental
                    oRates.OwnerAmount = wkEndOwner
                    oRates.Cost = wkEndCost
                    oRates.TPAmount = wkEndTPRate
                    oRates.TSAmount = wkEndTSRate
                Else
                    oRates.Amount = wkDayRate
                    oRates.RentalAmount = wkDayRental
                    oRates.OwnerAmount = wkDayOwner
                    oRates.TPAmount = wkDayTPRate
                    oRates.TSAmount = wkDayTSRate
                    oRates.Cost = wkDayCost
                End If
                oRates.Save()
                oRates = Nothing
                tempDate = tempDate.AddDays(1)
            Loop
            bBuilt = True
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return bBuilt
    End Function
    Public Property RateTableID() As Integer
        Get
            Return _RateTableID
        End Get
        Set(ByVal value As Integer)
            _RateTableID = value
        End Set
    End Property

    Public Property RateDate() As String
        Get
            Return _Date
        End Get
        Set(ByVal value As String)
            _Date = value
        End Set
    End Property

    Public Property Cost() As Decimal
        Get
            Return _Cost
        End Get
        Set(ByVal value As Decimal)
            _Cost = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Public Property RentalAmount() As Decimal
        Get
            Return _RentalAmount
        End Get
        Set(ByVal value As Decimal)
            _RentalAmount = value
        End Set
    End Property

    Public Property OwnerAmount() As Decimal
        Get
            Return _OwnerAmount
        End Get
        Set(ByVal value As Decimal)
            _OwnerAmount = value
        End Set
    End Property
    Public Property TPAmount() As Decimal
        Get
            Return _TPAmount
        End Get
        Set(ByVal value As Decimal)
            _TPAmount = value
        End Set
    End Property
    Public Property TSAmount() As Decimal
        Get
            Return _TSAmount
        End Get
        Set(ByVal value As Decimal)
            _TSAmount = value
        End Set
    End Property
    Public Property RateTableRateID() As Integer
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
