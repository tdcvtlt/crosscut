Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsProjectAreas
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Area As String = ""
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_ProjectAreas where AreaID = " & _ID, cn)
    End Sub

    Public Function Query(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            Dim sName(2) As String
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " AreaID, Area from t_ProjectAreas "
            If sFilterField <> "" Then
                If sFilterField = "Area" Then
                    sql += " where Area like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' "
                ElseIf sFilterField = "AreaID" Then
                    sql += " where AreaID like '" & sFilterValue & "%' "

                Else
                    sql += " where " & sFilterField & " like '" & sFilterValue & "%' "
                End If
            End If

            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select * from t_ProjectAreas order by Area"
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_ProjectAreas where AreaID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_ProjectAreas")
            If ds.Tables("t_ProjectAreas").Rows.Count > 0 Then
                dr = ds.Tables("t_ProjectAreas").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Area") Is System.DBNull.Value) Then _Area = dr("Area")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProjectAreas where AreaID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_ProjectAreas")
            If ds.Tables("t_ProjectAreas").Rows.Count > 0 Then
                dr = ds.Tables("t_ProjectAreas").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ProjectAreasInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Area", SqlDbType.VarChar, 0, "Area")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.Text, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@AreaID", SqlDbType.Int, 0, "AreaID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_ProjectAreas").NewRow
            End If
            Update_Field("Area", _Area, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_ProjectAreas").Rows.Count < 1 Then ds.Tables("t_ProjectAreas").Rows.Add(dr)
            da.Update(ds, "t_ProjectAreas")
            _ID = ds.Tables("t_ProjectAreas").Rows(0).Item("AreaID")
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
            oEvents.KeyField = "AreaID"
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

    Public Property Area() As String
        Get
            Return _Area
        End Get
        Set(ByVal value As String)
            _Area = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property AreaID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
    Public ReadOnly Property Error_Message As String
        Get
            Return _Err
        End Get
    End Property
End Class
