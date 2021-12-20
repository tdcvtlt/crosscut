Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccom2Resort
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AccomID As Integer = 0
    Dim _UnitTypeID As Integer = 0
    Dim _RateTableID As Integer = 0
    Dim _BD As String = ""
    Dim _MaxOccupancy As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Accom2Resort where Accom2ResortID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Accom2Resort where Accom2ResortID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Accom2Resort")
            If ds.Tables("t_Accom2Resort").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom2Resort").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AccomID") Is System.DBNull.Value) Then _AccomID = dr("AccomID")
        If Not (dr("UnitTypeID") Is System.DBNull.Value) Then _UnitTypeID = dr("UnitTypeID")
        If Not (dr("RateTableID") Is System.DBNull.Value) Then _RateTableID = dr("RateTableID")
        If Not (dr("BD") Is System.DBNull.Value) Then _BD = dr("BD")
        If Not (dr("MaxOccupancy") Is System.DBNull.Value) Then _MaxOccupancy = dr("MaxOccupancy")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Accom2Resort where Accom2ResortID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Accom2Resort")
            If ds.Tables("t_Accom2Resort").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom2Resort").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Accom2ResortInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.int, 0, "AccomID")
                da.InsertCommand.Parameters.Add("@UnitTypeID", SqlDbType.int, 0, "UnitTypeID")
                da.InsertCommand.Parameters.Add("@RateTableID", SqlDbType.int, 0, "RateTableID")
                da.InsertCommand.Parameters.Add("@BD", SqlDbType.varchar, 0, "BD")
                da.InsertCommand.Parameters.Add("@MaxOccupancy", SqlDbType.int, 0, "MaxOccupancy")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Accom2ResortID", SqlDbType.Int, 0, "Accom2ResortID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Accom2Resort").NewRow
            End If
            Update_Field("AccomID", _AccomID, dr)
            Update_Field("UnitTypeID", _UnitTypeID, dr)
            Update_Field("RateTableID", _RateTableID, dr)
            Update_Field("BD", _BD, dr)
            Update_Field("MaxOccupancy", _MaxOccupancy, dr)
            If ds.Tables("t_Accom2Resort").Rows.Count < 1 Then ds.Tables("t_Accom2Resort").Rows.Add(dr)
            da.Update(ds, "t_Accom2Resort")
            _ID = ds.Tables("t_Accom2Resort").Rows(0).Item("Accom2ResortID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "Accom2ResortID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Public Function Get_Accom_Resort_RoomTypes(ByVal accomID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select a.Accom2ResortID as ID, ut.ComboItem as UnitType, a.BD as Bedrooms, a.MaxOccupancy, rt.Name as RateTable from t_Accom2Resort a inner join t_ComboItems ut on a.UnitTypeID = ut.ComboItemID left outer join t_RateTable rt on a.RateTableID = rt.RateTableID where a.AccomID = " & accomID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property AccomID() As Integer
        Get
            Return _AccomID
        End Get
        Set(ByVal value As Integer)
            _AccomID = value
        End Set
    End Property

    Public Property UnitTypeID() As Integer
        Get
            Return _UnitTypeID
        End Get
        Set(ByVal value As Integer)
            _UnitTypeID = value
        End Set
    End Property

    Public Property RateTableID() As Integer
        Get
            Return _RateTableID
        End Get
        Set(ByVal value As Integer)
            _RateTableID = value
        End Set
    End Property

    Public Property BD() As String
        Get
            Return _BD
        End Get
        Set(ByVal value As String)
            _BD = value
        End Set
    End Property

    Public Property MaxOccupancy() As Integer
        Get
            Return _MaxOccupancy
        End Get
        Set(ByVal value As Integer)
            _MaxOccupancy = value
        End Set
    End Property

    Public Property Accom2ResortID() As Integer
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
