Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports Microsoft.VisualBasic

Public Class clsSalesRotor
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim dr As SqlDataReader
    Dim _Err As String = ""

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from v_SalesRotor where 1=2", cn)
    End Sub

    Public Function Build_Rotor(ByVal dept As String, ByVal weekly As Boolean, ByVal kcp As Boolean) As String
        Dim sRotor As String = ""
        Dim sRotorRep As String = ""
        Try
            sRotor = "<table border = '1'>"
            sRotor = sRotor & "<tr><td><B>Assigned</b></td><td><B>NAME</B></td><td><B>VPG</b></td><td><b>Most Recent Contract Date</B></td></tr>"
            sRotorRep = "<tr><td colspan = '4'><B>Full-Time Reps</B></td></tr>"
            'Timeout = 0
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandTimeout = 0
            If kcp Then
                If weekly = True Then
                    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotorWeekly sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype is not null order by VPG desc"
                Else
                    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotor sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype is not null order by VPG desc"
                End If
            Else
                If weekly = True Then
                    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotorWeeklyNOVA sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype is not null order by VPG desc"
                Else
                    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotorNOVA sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype is not null order by VPG desc"
                End If
            End If
            Dim vpgPassed As Boolean = False
            dr = cm.ExecuteReader
            Do While dr.Read
                If dr("SalesRotorType") = "Manager" Then
                    sRotor = sRotor & "<tr>"
                    sRotor = sRotor & "<td><input type = 'checkbox'></td>"
                    sRotor = sRotor & "<td>" & dr("Firstname") & " " & dr("Lastname") & "</td>"
                    sRotor = sRotor & "<td>N/A</td>"
                    sRotor = sRotor & "<td>N/A</td>"
                    sRotor = sRotor & "</td>"
                    sRotor = sRotor & "</tr>"
                Else
                    If dr("VPG") < 1520 And vpgPassed = False Then
                        vpgPassed = True
                        sRotorRep = sRotorRep & "<tr><td>=======</td><td>======================</td><td>========</td><td>=============================</td></tr>"
                    End If
                    sRotorRep = sRotorRep & "<tr>"
                    sRotorRep = sRotorRep & "<td><input type = 'checkbox'></td>"
                    sRotorRep = sRotorRep & "<td>" & dr("Firstname") & " " & dr("Lastname") & "</td>"
                    If IsDBNull(dr("VPG")) Then
                        sRotorRep = sRotorRep & "<td>$0.00</td>"
                    Else
                        sRotorRep = sRotorRep & "<td>" & FormatCurrency(dr("VPG"), 2) & "</td>"
                    End If
                    If dr("ContractID") & "" <> "" Then
                        sRotorRep = sRotorRep & "<td>" & dr("StatusDate") & " - # " & dr("ContractNumber") & " - " & dr("ConType") & "</td>"
                    Else
                        sRotorRep = sRotorRep & "<td>&nbsp; </td>"
                    End If
                    sRotorRep = sRotorRep & "</tr>"
                End If
            Loop

            dr.Close()
            sRotor = sRotor & sRotorRep & "</table>"



            'If weekly = True Then
            '    cm.CommandText = "Select * from v_SalesRotorWeekly where Dept = '" & dept & "' and salesrotortype = 'Manager'"
            'Else
            '    cm.CommandText = "Select * from v_SalesRotor where Dept = '" & dept & "' and salesrotortype = 'Manager'"
            'End If
            'dr = cm.ExecuteReader
            'Do While dr.Read
            '    sRotor = sRotor & "<tr>"
            '    sRotor = sRotor & "<td><input type = 'checkbox'></td>"
            '    sRotor = sRotor & "<td>" & dr("Firstname") & " " & dr("Lastname") & "</td>"
            '    sRotor = sRotor & "<td>N/A</td>"
            '    sRotor = sRotor & "<td>N/A</td>"
            '    sRotor = sRotor & "</td>"
            'Loop
            'dr.Close()

            'Senior Reps
            'If weekly = True Then
            '    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotorWeekly sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype = 'SeniorRep' order by VPG desc"
            'Else
            '    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotor sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype = 'SeniorRep' order by VPG desc"
            'End If
            'dr = cm.ExecuteReader
            'If dr.HasRows Then
            '    sRotor = sRotor & "<tr><td colspan = '4'><B>Senior Sales Reps</B></td></tr>"
            'End If
            'Do While dr.Read
            '    sRotor = sRotor & "<tr>"
            '    sRotor = sRotor & "<td><input type = 'checkbox'></td>"
            '    sRotor = sRotor & "<td>" & dr("Firstname") & " " & dr("Lastname") & "</td>"
            '    If IsDBNull(dr("VPG")) Then
            '        sRotor = sRotor & "<td>$0.00</td>"
            '    Else
            '        sRotor = sRotor & "<td>" & FormatCurrency(dr("VPG"), 2) & "</td>"
            '    End If
            '    If dr("ContractID") & "" <> "" Then
            '        sRotor = sRotor & "<td>" & dr("StatusDate") & " - # " & dr("ContractNumber") & " - " & dr("ConType") & "</td>"
            '    Else
            '        sRotor = sRotor & "<td>&nbsp; </td>"
            '    End If
            '    sRotor = sRotor & "</tr>"
            'Loop
            'dr.Close()
            'Full Time Reps
            'sRotor = sRotor & "<tr><td colspan = '4'><B>Full-Time Reps</B></td></tr>"
            'If weekly = True Then
            '    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotorWeekly sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype = 'FullTime' order by VPG desc"
            'Else
            '    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotor sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype = 'FullTime' order by VPG desc"
            'End If
            'dr = cm.ExecuteReader
            'Dim vpgPassed As Boolean = False
            'Do While dr.Read
            '    If dr("VPG") < 1520 And vpgPassed = False Then
            '        vpgPassed = True
            '        sRotor = sRotor & "<tr><td>=======</td><td>======================</td><td>========</td><td>=============================</td></tr>"
            '    End If
            '    sRotor = sRotor & "<tr>"
            '    sRotor = sRotor & "<td><input type = 'checkbox'></td>"
            '    sRotor = sRotor & "<td>" & dr("Firstname") & " " & dr("Lastname") & "</td>"
            '    If IsDBNull(dr("VPG")) Then
            '        sRotor = sRotor & "<td>$0.00</td>"
            '    Else
            '        sRotor = sRotor & "<td>" & FormatCurrency(dr("VPG"), 2) & "</td>"
            '    End If
            '    If dr("ContractID") & "" <> "" Then
            '        sRotor = sRotor & "<td>" & dr("StatusDate") & " - # " & dr("ContractNumber") & " - " & dr("ConType") & "</td>"
            '    Else
            '        sRotor = sRotor & "<td>&nbsp; </td>"
            '    End If
            '    sRotor = sRotor & "</tr>"
            'Loop
            'dr.Close()

            'Part Time Reps
            'If weekly = True Then
            '    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotorWeekly sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype = 'PartTime' order by VPG desc"
            'Else
            '    cm.CommandText = "Select sr.*, c.StatusDate, c.ContractNumber, ct.ComboItem as ConType from v_SalesRotor sr left outer join t_Contract c on sr.ContractiD = c.COntractID left outer join t_Comboitems ct on c.TypeID = ct.ComboItemID where Dept = '" & dept & "' and salesrotortype = 'PartTime' order by VPG desc"
            'End If
            'dr = cm.ExecuteReader
            'If dr.HasRows Then
            '    sRotor = sRotor & "<tr><td colspan = '4'><B>Part-Time Reps</B></td></tr>"
            'End If
            'Do While dr.Read
            '    sRotor = sRotor & "<tr>"
            '    sRotor = sRotor & "<td><input type = 'checkbox'></td>"
            '    sRotor = sRotor & "<td>" & dr("Firstname") & " " & dr("Lastname") & "</td>"
            '    If IsDBNull(dr("VPG")) Then
            '        sRotor = sRotor & "<td>$0.00</td>"
            '    Else
            '        sRotor = sRotor & "<td>" & FormatCurrency(dr("VPG"), 2) & "</td>"
            '    End If
            '    If dr("ContractID") & "" <> "" Then
            '        sRotor = sRotor & "<td>" & dr("StatusDate") & " - # " & dr("ContractNumber") & " - " & dr("ConType") & "</td>"
            '    Else
            '        sRotor = sRotor & "<td>&nbsp; </td>"
            '    End If
            '    sRotor = sRotor & "</tr>"
            'Loop
            'dr.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return sRotor
    End Function

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property
End Class
