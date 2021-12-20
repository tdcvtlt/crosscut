Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageAdditionalCost
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _Amount As Decimal = 0
    Dim _Priority As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageAdditionalCost where PackageNightRateID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageAdditionalCost where PackageNightRateID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageAdditionalCost")
            If ds.Tables("t_PackageAdditionalCost").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageAdditionalCost").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("Amount") Is System.DBNull.Value) Then _Amount = dr("Amount")
        If Not (dr("Priority") Is System.DBNull.Value) Then _Priority = dr("Priority")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageAdditionalCost where PackageNightRateID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageAdditionalCost")
            If ds.Tables("t_PackageAdditionalCost").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageAdditionalCost").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageAdditionalCostInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.datetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.datetime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@Amount", SqlDbType.money, 0, "Amount")
                da.InsertCommand.Parameters.Add("@Priority", SqlDbType.int, 0, "Priority")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageNightRateID", SqlDbType.Int, 0, "PackageNightRateID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageAdditionalCost").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("Amount", _Amount, dr)
            Update_Field("Priority", _Priority, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_PackageAdditionalCost").Rows.Count < 1 Then ds.Tables("t_PackageAdditionalCost").Rows.Add(dr)
            da.Update(ds, "t_PackageAdditionalCost")
            _ID = ds.Tables("t_PackageAdditionalCost").Rows(0).Item("PackageNightRateID")
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
            oEvents.KeyField = "PackageNightRateID"
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

    Public Function List_Add_Costs(ByVal pkgID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select PackageNightRateID as ID, StartDate, EndDate, Amount, Priority, Active from t_packageAdditionalCost where packageID = " & pkgID
        Return ds
    End Function

    Public Function get_AdditionalCost(ByVal packageID As Integer, ByVal sDate As Date, ByVal eDate As Date) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Day")
        dt.Columns.Add("Rate")
        Dim tempDate As Date = sDate
        Dim amt As Double = 0
        Dim bFound As Boolean = False
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select * from t_PackageAdditionalCost where packageid = " & packageID & " and ((startdate between '" & sDate & "' and '" & eDate & "') or (enddate between '" & sDate & "' and '" & eDate & "') or (startdate <= '" & sDate & "' and enddate >= '" & eDate & "') or (enddate >= '" & sDate & "' and enddate <= '" & eDate & "')) order by priority desc"
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        da.Fill(ds, "Dates")

        Do While tempDate <= eDate
            amt = 0
            bFound = False
            If ds.Tables("Dates").Rows.Count > 0 Then
                For i = 0 To ds.Tables("Dates").Rows.Count - 1
                    If DateTime.Compare(CDate(tempDate), CDate(ds.Tables("Dates").Rows(i).Item("StartDate"))) >= 0 And DateTime.Compare(CDate(tempDate), CDate(ds.Tables("Dates").Rows(i).Item("EndDate"))) <= 0 Then
                        amt = ds.Tables("Dates").Rows(i).Item("Amount")
                        Exit For
                    End If
                Next
            End If
            dr = dt.NewRow
            dr.Item("Day") = tempDate
            dr.Item("Rate") = amt
            dt.Rows.Add(dr)
            tempDate = tempDate.AddDays(1)
        Loop
        da.Dispose()
        If cn.State <> ConnectionState.Closed Then cn.Close()

        Return dt
    End Function

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
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

    Public Property Priority() As Integer
        Get
            Return _Priority
        End Get
        Set(ByVal value As Integer)
            _Priority = value
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property PackageNightRateID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
End Class
