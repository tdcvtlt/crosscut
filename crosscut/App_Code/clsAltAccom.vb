Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsAltAccom
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _TourID As Integer = 0

    Dim _Err As String = ""


    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader


    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Contract where ContractID = " & _ID, cn)

    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Contract where contractid = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Contract")

            If ds.Tables("Contract").Rows.Count > 0 Then
                dr = ds.Tables("Contract").Rows(0)
                'Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub
End Class
