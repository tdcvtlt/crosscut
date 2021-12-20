Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAccom2RoomType
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _AccomID As Integer = 0
    Dim _RoomTypeID As Integer = 0
    Dim _RateTableID As Integer = 0
    Dim _MaxOccupancy As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Accom2RoomType where AccomID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Accom2RoomType where AccomID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Accom2RoomType")
            If ds.Tables("t_Accom2RoomType").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom2RoomType").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("AccomID") Is System.DBNull.Value) Then _AccomID = dr("AccomID")
        If Not (dr("RoomTypeID") Is System.DBNull.Value) Then _RoomTypeID = dr("RoomTypeID")
        If Not (dr("RateTableID") Is System.DBNull.Value) Then _RateTableID = dr("RateTableID")
        If Not (dr("MaxOccupancy") Is System.DBNull.Value) Then _MaxOccupancy = dr("MaxOccupancy")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Accom2RoomType where AccomID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Accom2RoomType")
            If ds.Tables("t_Accom2RoomType").Rows.Count > 0 Then
                dr = ds.Tables("t_Accom2RoomType").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Accom2RoomTypeInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.Int, 0, "AccomID")
                da.InsertCommand.Parameters.Add("@RoomTypeID", SqlDbType.Int, 0, "RoomTypeID")
                da.InsertCommand.Parameters.Add("@RateTableID", SqlDbType.Int, 0, "RateTableID")
                da.InsertCommand.Parameters.Add("@MaxOccupancy", SqlDbType.Int, 0, "MaxOccupancy")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AccomID", SqlDbType.Int, 0, "AccomID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Accom2RoomType").NewRow
            End If
            Update_Field("AccomID", _AccomID, dr)
            Update_Field("RoomTypeID", _RoomTypeID, dr)
            Update_Field("RateTableID", _RateTableID, dr)
            Update_Field("MaxOccupancy", _MaxOccupancy, dr)
            If ds.Tables("t_Accom2RoomType").Rows.Count < 1 Then ds.Tables("t_Accom2RoomType").Rows.Add(dr)
            da.Update(ds, "t_Accom2RoomType")
            _ID = ds.Tables("t_Accom2RoomType").Rows(0).Item("AccomID")
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
            oEvents.KeyField = "AccomID"
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

    Public Function Get_Accom_Hotel_RoomTypes(ByVal accomID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select a.Accom2RoomTypeID as ID, rmt.ComboItem as RoomType, a.MaxOccupancy, rt.Name as RateTable from t_Accom2RoomType a inner join t_ComboItems rmt on a.RoomTypeID = rmt.ComboItemID left outer join t_RateTable rt on a.RateTableID = rt.RateTableID where a.AccomID = " & accomID
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Property AccomID() As Integer
        Get
            Return _AccomID
        End Get
        Set(ByVal value As Integer)
            _AccomID = value
        End Set
    End Property

    Public Property RoomTypeID() As Integer
        Get
            Return _RoomTypeID
        End Get
        Set(ByVal value As Integer)
            _RoomTypeID = value
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

    Public Property MaxOccupancy() As Integer
        Get
            Return _MaxOccupancy
        End Get
        Set(ByVal value As Integer)
            _MaxOccupancy = value
        End Set
    End Property


    Public Property Accom2RoomTypeID() As Integer
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
