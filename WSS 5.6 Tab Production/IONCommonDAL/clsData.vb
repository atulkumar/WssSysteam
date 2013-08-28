Imports Microsoft.VisualBasic
Imports System.Configuration
Imports ION.Logging.EventLogging
Imports System.Data.SqlClient
Imports System
Imports System.Data


Public Class clsData

    Private Shared Factory As System.Data.Common.DbProviderFactory
    Private Shared cnConnection As System.Data.Common.DbConnection
    Private Shared dsResults As System.Data.DataSet
    Private Shared sqADP As SqlClient.SqlDataAdapter
    Private Shared sqCMD As SqlClient.SqlCommand
    Private Shared mstrConnectionString As String

    Public Shared Property ConnectionString() As String
        Get
            Return mstrConnectionString
        End Get
        Set(ByVal value As String)
            mstrConnectionString = value
        End Set
    End Property

    Public Shared WriteOnly Property DBProvider() As String
        Set(ByVal value As String)
            Factory = System.Data.Common.DbProviderFactories.GetFactory(value)
        End Set
    End Property

    ''' <summary>
    ''' Takes multiple queries and table name along with the values to be updated in the table
    ''' </summary>
    ''' <param name="UpdateQueryandTable">First value should be a unique table name and 
    ''' second value should be condition in the where clause</param>
    ''' <param name="ColumnandData">First value should be Column name and second value should be Column data.</param>
    ''' <returns>True is sucessful and false if not</returns>
    ''' <remarks></remarks>
    Public Shared Function Update(ByVal UpdateQueryandTable As System.Collections.Generic.Dictionary(Of String, String), ByVal ColumnandData As System.Collections.Generic.Dictionary(Of String, Object)) As Boolean
        Try
            Using cnConnection As System.Data.Common.DbConnection = Connection, _
                    cmdCommand As System.Data.Common.DbCommand = Command, _
                    adAdapter As System.Data.Common.DbDataAdapter = DataAdapter, _
                    ds As New System.Data.DataSet

                cmdCommand.Connection = cnConnection
                cnConnection.ConnectionString = ConnectionString

                ' Iterating through tables
                For Each kvpQT As System.Collections.Generic.KeyValuePair(Of String, String) In UpdateQueryandTable
                    cmdCommand.CommandText = "select * from " & kvpQT.Key & " where " & kvpQT.Value
                    adAdapter.SelectCommand = cmdCommand
                    adAdapter.Fill(ds, kvpQT.Key)

                    ' Iterating through Column to be updated and the data to be updated
                    For Each kvpCD As System.Collections.Generic.KeyValuePair(Of String, Object) In ColumnandData
                        For inti As Integer = 0 To ds.Tables(kvpQT.Key).Rows.Count - 1
                            ds.Tables(kvpQT.Key).Rows(inti).Item(kvpCD.Key) = kvpCD.Value
                        Next inti
                    Next kvpCD

                    Dim dsChanges = ds.GetChanges
                    If IsNothing(dsChanges) Then
                        Return False
                    Else
                        Dim cmdCommandBuilder As System.Data.Common.DbCommandBuilder = Factory.CreateCommandBuilder
                        cmdCommandBuilder.DataAdapter = adAdapter
                        adAdapter.Update(ds, kvpQT.Key)
                        Return True
                    End If
                Next kvpQT
            End Using
        Catch ex As Exception
            CreateLog("clsData", "Update-62", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UpdateSQL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Update(ByVal UpdateSQL As String) As Boolean

        Using cnConnection As System.Data.Common.DbConnection = Connection, _
                    cmdCommand As System.Data.Common.DbCommand = Command
            Try
                cnConnection.ConnectionString = ConnectionString
                cmdCommand.Connection = cnConnection
                cmdCommand.CommandText = UpdateSQL
                cnConnection.Open()
                cmdCommand.ExecuteNonQuery()
                Return True
            Catch ex As Exception
                CreateLog("clsData", "Update-85", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            Finally
                cnConnection.Close()
            End Try
        End Using
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UpdateDatatable"></param>
    ''' <param name="SelectQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Update(ByVal UpdateDatatable As DataTable, ByVal SelectQuery As String) As Boolean

        Using cnConnection As System.Data.Common.DbConnection = Connection, _
                    cmdCommand As System.Data.Common.DbCommand = Command, _
                    adAdapter As System.Data.Common.DbDataAdapter = DataAdapter
            Try
                cmdCommand.Connection = cnConnection
                cnConnection.ConnectionString = ConnectionString

                cmdCommand.CommandText = SelectQuery
                Dim dtTemp As New DataTable
                adAdapter.FillLoadOption = LoadOption.OverwriteChanges
                adAdapter.AcceptChangesDuringUpdate = True
                adAdapter.SelectCommand = cmdCommand
                adAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey

                adAdapter.Fill(dtTemp)
                dtTemp.Merge(UpdateDatatable, False)

                Dim dsChanges = dtTemp.GetChanges
                If IsNothing(dsChanges) Then
                    Return False
                Else
                    Dim cmdCommandBuilder As System.Data.Common.DbCommandBuilder = Factory.CreateCommandBuilder
                    cmdCommandBuilder.DataAdapter = adAdapter
                    Dim i As Integer
                    i = adAdapter.Update(dtTemp)
                    MsgBox(i)
                    Return True
                End If
            Catch ex As Exception
                CreateLog("clsData", "Update-131", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            Finally
                cnConnection.Close()
            End Try
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBTable"></param>
    ''' <param name="SQLSelect"></param>
    ''' <param name="ColumnName"></param>
    ''' <param name="RowData"></param>
    ''' <param name="LogID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Update(ByVal DBTable As String, ByVal SQLSelect As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal LogID As String) As Boolean

        Try
            Using cnConnection As System.Data.Common.DbConnection = Connection, _
                   cmdCommand As System.Data.Common.DbCommand = Command, _
                   adAdapter As System.Data.Common.DbDataAdapter = DataAdapter

                cmdCommand.Connection = cnConnection
                cnConnection.ConnectionString = ConnectionString

                cmdCommand.CommandText = SQLSelect
                Dim dtTemp As New DataTable
                adAdapter.FillLoadOption = LoadOption.OverwriteChanges
                adAdapter.AcceptChangesDuringUpdate = True
                adAdapter.SelectCommand = cmdCommand

                dsResults = New System.Data.DataSet
                adAdapter.Fill(dsResults, DBTable)
                Dim sqRow As DataRow = dsResults.Tables(DBTable).Rows(0)
                For IntI As Integer = 0 To ColumnName.Count - 1
                    If IsDBNull(RowData.Item(IntI)) = False And IsNothing(RowData.Item(IntI)) = False Then
                        sqRow(ColumnName.Item(IntI)) = RowData.Item(IntI)
                    Else
                        sqRow(ColumnName.Item(IntI)) = System.DBNull.Value
                    End If
                Next
                Dim DatasetChanges = dsResults.GetChanges
                If IsNothing(DatasetChanges) Then
                    dsResults.Dispose()
                    Return False
                Else
                    Dim cmdBuilder As System.Data.Common.DbCommandBuilder = Factory.CreateCommandBuilder
                    cmdBuilder.DataAdapter = adAdapter
                    adAdapter.Update(dsResults, DBTable)
                    adAdapter.Dispose()
                    dsResults.Dispose()
                    cmdBuilder.Dispose()
                    Return True
                End If
            End Using
        Catch ex As Exception
            sqADP.Dispose()
            dsResults.Dispose()
            CreateLog("clsSQL", "Update", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Pass the datatable to the function and it will save all the rows. Column name in the Datatable has to have the same name as it is in Database.
    ''' </summary>
    ''' <param name="DataTableInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveWithoutPrimary(ByVal DataTableInfo As DataSet) As Boolean
        Try
            'Loop for tables in dataset
            For inti As Integer = 0 To DataTableInfo.Tables.Count - 1
                'Loop for row values
                For intj As Integer = 0 To DataTableInfo.Tables(inti).Rows.Count - 1
                    ' Looping through columns to create the insert query
                    Dim strInsert As String = "Insert into " & DataTableInfo.Tables(inti).TableName & "("
                    Dim strValues As String = ") Values("

                    For intk As Integer = 0 To DataTableInfo.Tables(inti).Columns.Count - 1
                        strInsert &= DataTableInfo.Tables(inti).Columns(intk).ColumnName & ","

                        If IsDBNull(DataTableInfo.Tables(inti).Rows(intj).Item(intk)) = False And IsNothing(DataTableInfo.Tables(inti).Rows(intj).Item(intk)) = False Then
                            strValues &= "'" & DataTableInfo.Tables(inti).Rows(intj).Item(intk) & "',"
                        Else
                            strValues &= DBNull.Value & ","
                        End If
                    Next intk
                    strValues = strValues.Remove(strValues.Length - 1, 1)
                    strValues &= ")"

                    strInsert = strInsert.Remove(strInsert.Length - 1, 1)
                    strInsert &= strValues

                    'Save
                    Save(strInsert, True)
                Next intj
            Next inti
            Return True
        Catch ex As Exception
            CreateLog("clsData", "SaveWithoutPrimary-176", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="InsertQuery"></param>
    ''' <param name="Serializable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(ByVal InsertQuery As String, ByVal Serializable As Boolean) As Boolean
        Using cnConnection As System.Data.Common.DbConnection = Connection, _
        cmdCommand As System.Data.Common.DbCommand = Command

            cnConnection.ConnectionString = ConnectionString
            Dim dbt As System.Data.Common.DbTransaction
            cmdCommand.CommandText = InsertQuery
            cmdCommand.Connection = cnConnection
            Try
                cnConnection.Open()
                If Serializable = True Then
                    dbt = cnConnection.BeginTransaction(System.Data.IsolationLevel.Serializable)
                Else
                    dbt = cnConnection.BeginTransaction(System.Data.IsolationLevel.Unspecified)
                End If
                cmdCommand.Transaction = dbt
                cmdCommand.ExecuteNonQuery()
                dbt.Commit()
                Return True
            Catch ex As System.Exception
                dbt.Rollback()
                CreateLog("clsData", "Save-209", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            Finally
                dbt.Dispose()
            End Try
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DataTableInfo"></param>
    ''' <param name="TableName"></param>
    ''' <param name="Query"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(ByVal DataTableInfo As DataTable, ByVal TableName As String, ByVal Query As String) As Boolean
        Try
            Using cnConnection As System.Data.Common.DbConnection = Connection, _
                    cmdCommand As System.Data.Common.DbCommand = Command, _
                    adAdapter As System.Data.Common.DbDataAdapter = DataAdapter, _
                    dt As New System.Data.DataTable

                cmdCommand.Connection = cnConnection
                cnConnection.ConnectionString = ConnectionString
                cmdCommand.CommandText = Query
                adAdapter.SelectCommand = cmdCommand
                adAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey
                adAdapter.Fill(dt)
                dt.Merge(DataTableInfo, True)
                Dim dtChanges = dt.GetChanges
                If IsNothing(dtChanges) = False Then
                    Dim sqcmdb As New SqlClient.SqlCommandBuilder(adAdapter)
                    adAdapter.Update(dt)
                End If
            End Using
            Return True
        Catch ex As Exception
            CreateLog("clsData", "Save-247", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Save a single row into the table return true is saved
    ''' </summary>
    ''' <param name="ColumnandData">First value should be a column name and second value should be row data
    ''' </param>
    ''' <param name="TableName">Name of the table where row will be saved</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(ByVal ColumnandData As System.Collections.Generic.Dictionary(Of String, Object), ByVal TableName As String) As Boolean
        Using cnConnection As System.Data.Common.DbConnection = Connection, _
        cmdCommand As System.Data.Common.DbCommand = Command, _
        adAdapter As System.Data.Common.DbDataAdapter = DataAdapter

            cmdCommand.CommandText = "Select top 1 * from " & TableName
            cmdCommand.Connection = cnConnection
            cnConnection.ConnectionString = ConnectionString
            Try
                Dim dt As New System.Data.DataTable
                adAdapter.SelectCommand = cmdCommand
                adAdapter.FillSchema(dt, System.Data.SchemaType.Source)
                Dim dr As System.Data.DataRow = dt.NewRow
                For Each kvp As System.Collections.Generic.KeyValuePair(Of String, Object) In ColumnandData
                    If kvp.Value Is DBNull.Value Then
                        dr(kvp.Key) = kvp.Value
                    Else
                        dr(kvp.Key) = kvp.Value.ToString
                    End If
                Next
                dt.Rows.Add(dr)
                Dim cmdCommandBuilder As System.Data.SqlClient.SqlCommandBuilder = CommandBuilder
                cmdCommandBuilder.DataAdapter = adAdapter
                adAdapter.Update(dt)
                Return True
            Catch ex As Exception
                CreateLog("clsData", "Save-282", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Executes the query in the database with transaction support
    ''' </summary>
    ''' <param name="InsertQuery">Insert query to be fired in the database</param>
    ''' <param name="TransactionLevel" >Level of transaction to be applied</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(ByVal TableName As String, ByVal InsertQuery As String, ByVal TransactionLevel As System.Data.IsolationLevel) As Int32
        Using cnConnection As System.Data.Common.DbConnection = Connection, _
              cmdCommand As System.Data.Common.DbCommand = Command

            cnConnection.ConnectionString = ConnectionString
            Dim dbt As System.Data.Common.DbTransaction
            Dim tagId As Integer = 0
            cmdCommand.CommandText = InsertQuery
            cmdCommand.Connection = cnConnection
            Try
                cnConnection.Open()
                dbt = cnConnection.BeginTransaction(TransactionLevel)
                cmdCommand.Transaction = dbt
                cmdCommand.ExecuteNonQuery()
                cmdCommand.CommandText = "SELECT @@IDENTITY FROM " & TableName 'return saved primary key ID   
                Return Convert.ToInt32(cmdCommand.ExecuteScalar.ToString)
            Catch ex As System.Exception
                dbt.Rollback()
                CreateLog("clsData", "Save-313", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            Finally
                dbt.Commit()
                dbt.Dispose()
            End Try
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBTable"></param>
    ''' <param name="ColumnName"></param>
    ''' <param name="RowData"></param>
    ''' <param name="LogID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveMulti(ByVal DBTable As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal LogID As String) As Boolean
        Try
            Dim strInsertSQL As String
            strInsertSQL = "select top 5 * from " & DBTable
            sqADP = New SqlClient.SqlDataAdapter(strInsertSQL, ConnectionString)
            dsResults = New System.Data.DataSet
            sqADP.Fill(dsResults, DBTable)
            Dim sqRow As DataRow
            For IntI As Integer = 0 To RowData.Count - 1
                sqRow = dsResults.Tables(DBTable).NewRow
                For IntJ As Integer = 0 To ColumnName.Count - 1
                    If Not IsNothing(RowData.Item(IntI)) Then
                        sqRow(ColumnName.Item(IntJ)) = CType(RowData.Item(IntI), ArrayList).Item(IntJ)
                    End If
                Next
                dsResults.Tables(DBTable).Rows.Add(sqRow)
            Next
            Dim cmdBuilder As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqADP)
            sqADP.Update(dsResults, DBTable)
            sqADP.Dispose()
            dsResults.Dispose()
            cmdBuilder.Dispose()
            Return True
        Catch ex As Exception
            sqADP.Dispose()
            dsResults.Dispose()
            CreateLog("clsData", "SaveMulti-358", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    Public Shared Function Save(ByVal DBTable As String, ByVal ColumnName As ArrayList, ByVal RowData As ArrayList, ByVal LogID As String) As Boolean
        Try
            Dim strInsertSQL As String
            strInsertSQL = "select top 5 * from " & DBTable
            sqADP = New SqlClient.SqlDataAdapter(strInsertSQL, ConnectionString)
            dsResults = New System.Data.DataSet
            sqADP.Fill(dsResults, DBTable)
            Dim sqRow As DataRow

            sqRow = dsResults.Tables(DBTable).NewRow
            For IntJ As Integer = 0 To ColumnName.Count - 1
                If Not IsNothing(RowData.Item(IntJ)) Then
                    sqRow(ColumnName.Item(IntJ)) = RowData.Item(IntJ)
                End If
            Next
            dsResults.Tables(DBTable).Rows.Add(sqRow)

            Dim cmdBuilder As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(sqADP)
            sqADP.Update(dsResults, DBTable)
            sqADP.Dispose()
            dsResults.Dispose()
            cmdBuilder.Dispose()
            Return True
        Catch ex As Exception
            sqADP.Dispose()
            dsResults.Dispose()
            CreateLog("clsData", "SaveMulti-358", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Delete(s) the records from the table with transaction support
    ''' </summary>
    ''' <param name="DeleteQuery"></param>
    ''' <param name="TransactionLevel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Delete(ByVal DeleteQuery As String, ByVal TransactionLevel As System.Data.IsolationLevel) As Boolean
        Using cnConnection As System.Data.Common.DbConnection = Connection, cmdCommand As System.Data.Common.DbCommand = Command
            cnConnection.ConnectionString = ConnectionString
            Dim dbt As System.Data.Common.DbTransaction
            cmdCommand.CommandText = DeleteQuery
            cmdCommand.Connection = cnConnection
            Try
                cnConnection.Open()
                dbt = cnConnection.BeginTransaction(TransactionLevel)
                cmdCommand.Transaction = dbt
                cmdCommand.ExecuteNonQuery()
                dbt.Commit()
                Return True
            Catch ex As System.Exception
                dbt.Rollback()
                CreateLog("clsData", "Delete-385", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            Finally
                dbt.Dispose()
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Executes the query, fills the table and return it
    ''' </summary>
    ''' <param name="SearchQuery">Select query to search the database</param>
    ''' <param name="TableName">Table name which will be used to return DataTable</param>
    ''' <returns>Datatable with the name specified in the parameter</returns>
    ''' <remarks></remarks>
    Public Shared Function Search(ByVal SearchQuery As String, ByVal TableName As String) As System.Data.DataTable
        Try
            Dim cnConnection As System.Data.Common.DbConnection = Connection
            cnConnection.ConnectionString = ConnectionString
            Dim cmdCommand As System.Data.Common.DbCommand = Command
            cmdCommand.CommandText = SearchQuery
            cmdCommand.Connection = cnConnection
            cnConnection.Open()
            Dim dt As New System.Data.DataTable
            dt.TableName = TableName
            dt.Load(cmdCommand.ExecuteReader)
            cnConnection.Close()
            Return dt
        Catch ex As Exception
            CreateLog("clsData", "Search-414", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    Public Shared Function ExecuteSP(ByVal SPName As String, ByVal TableName As String) As System.Data.DataTable
        Try
            Dim cnConnection As System.Data.Common.DbConnection = Connection
            cnConnection.ConnectionString = ConnectionString
            Dim cmdCommand As System.Data.Common.DbCommand = Command
            cmdCommand.CommandType = CommandType.StoredProcedure
            cmdCommand.CommandText = SPName
            cmdCommand.Connection = cnConnection
            cnConnection.Open()
            Dim dt As New System.Data.DataTable
            dt.TableName = TableName
            Dim adpt As New SqlDataAdapter(cmdCommand)
            adpt.Fill(dt)
            cnConnection.Close()
            Return dt
        Catch ex As Exception
            CreateLog("clsData", "Search-414", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    Public Shared Function ExecuteSPWithParameters(ByVal SPName As String, ByVal ShopId As Int32, ByVal ShopCategory As Int32, ByVal ShopType As Int32, ByVal action As String) As Boolean
        Try
            Dim cnConnection As System.Data.Common.DbConnection = Connection
            cnConnection.ConnectionString = ConnectionString
            Dim cmdCommand As System.Data.Common.DbCommand = Command
            cmdCommand.CommandType = CommandType.StoredProcedure
            cmdCommand.CommandText = SPName
            cmdCommand.Parameters.Add(New SqlParameter("@ShopId", SqlDbType.Int, 10))
            cmdCommand.Parameters.Add(New SqlParameter("@ShopCategory", SqlDbType.Int, 10))
            cmdCommand.Parameters.Add(New SqlParameter("@ShopType", SqlDbType.Int, 10))
            cmdCommand.Parameters.Add(New SqlParameter("@action", SqlDbType.VarChar, 12))
            cmdCommand.Parameters("@ShopId").Value = ShopId
            cmdCommand.Parameters("@ShopCategory").Value = ShopCategory
            cmdCommand.Parameters("@ShopType").Value = ShopType
            cmdCommand.Parameters("@action").Value = action

            cmdCommand.Connection = cnConnection
            cnConnection.Open()
            cmdCommand.ExecuteNonQuery()
            cnConnection.Close()
            Return True
        Catch ex As Exception
            CreateLog("clsData", "Search-414", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function
  
    Public Shared Function SearchDS(ByVal SQLSelect As String) As System.Data.DataSet
        Try
            Dim dsResults As New DataSet
            Using cnConnection As System.Data.Common.DbConnection = Connection, _
                               cmdCommand As System.Data.Common.DbCommand = Command, _
                               adAdapter As System.Data.Common.DbDataAdapter = DataAdapter
                cnConnection.ConnectionString = ConnectionString
                cmdCommand.Connection = cnConnection
                cmdCommand.CommandText = SQLSelect
                adAdapter.SelectCommand = cmdCommand
                adAdapter.Fill(dsResults)
            End Using
            Return dsResults
        Catch ex As Exception
            CreateLog("clsData", "Search-415", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Takes multiple queries and table name and returns them in a dataset
    ''' </summary>
    ''' <param name="QueryandTableNames">First value should be a unique table name and 
    ''' second value should be query</param>
    ''' <returns>Dataset</returns>
    ''' <remarks></remarks>
    Public Shared Function Search(ByVal QueryandTableNames As System.Collections.Generic.Dictionary(Of String, String)) As System.Data.DataSet
        Try
            Dim ds As New System.Data.DataSet
            Using cnConnection As System.Data.Common.DbConnection = Connection, _
                    cmdCommand As System.Data.Common.DbCommand = Command, _
                    adAdapter As System.Data.Common.DbDataAdapter = DataAdapter
                cnConnection.ConnectionString = ConnectionString
                cmdCommand.Connection = cnConnection
                For Each kvp As System.Collections.Generic.KeyValuePair(Of String, String) In QueryandTableNames
                    cmdCommand.CommandText = kvp.Value
                    adAdapter.SelectCommand = cmdCommand
                    adAdapter.Fill(ds, kvp.Key)
                Next
            End Using
            Return ds
        Catch ex As Exception
            CreateLog("clsData", "Search-442", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Executes the query and return a datareader. Close the connection seperately using 
    ''' CloseConnection method 
    ''' </summary>
    ''' <param name="SearchQuery">Select query to search the database</param>
    ''' <returns>Datareader</returns>
    ''' <remarks>Connection object will not be closed here as datareader needs open connection
    ''' close the connection seperately after finishing the work 
    ''' <example> Shows how to close the connection
    ''' <code>
    ''' If objDB.IsConnected = True Then
    ''' objDB.CloseConnection()
    ''' End If
    ''' </code>
    ''' </example>
    ''' </remarks>
    Public Shared Function Search(ByVal SearchQuery As String) As System.Data.Common.DbDataReader
        cnConnection = Connection
        Dim cmdCommand As System.Data.Common.DbCommand = Command
        cnConnection.ConnectionString = ConnectionString
        cmdCommand.CommandText = SearchQuery
        cmdCommand.Connection = cnConnection
        cnConnection.Open()
        Return cmdCommand.ExecuteReader
    End Function

    ''' <summary>
    ''' Executes the query and return the datareader with single row. Close the connection seperately using 
    ''' CloseConnection method 
    ''' </summary>
    ''' <param name="SearchQuery">Select query to search the database</param>
    ''' <returns>Datareader</returns>
    ''' <remarks>Connection object will not be closed here as datareader needs open connection
    ''' close the connection seperately after finishing the work 
    ''' <example> Shows how to close the connection
    ''' <code>
    ''' If objDB.IsConnected = True Then
    ''' objDB.CloseConnection()
    ''' End If
    ''' </code>
    ''' </example>
    ''' </remarks>
    Public Shared Function SearchSingleRow(ByVal SearchQuery As String) As System.Data.Common.DbDataReader
        cnConnection = Connection
        Dim cmdCommand As System.Data.Common.DbCommand = Command
        cnConnection.ConnectionString = ConnectionString
        cmdCommand.CommandText = SearchQuery
        cmdCommand.Connection = cnConnection
        cnConnection.Open()
        Return cmdCommand.ExecuteReader(System.Data.CommandBehavior.SingleRow)
    End Function

    ''' <summary>
    ''' Returns the first column of the query passed
    ''' </summary>
    ''' <param name="SearchQuery">Select query to search the database</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SearchSingleValue(ByVal SearchQuery As String) As Object
        Try
            Using cnConnection As System.Data.Common.DbConnection = Connection, _
                                  cmdCommand As System.Data.Common.DbCommand = Command

                cnConnection.ConnectionString = ConnectionString
                If cnConnection.State = ConnectionState.Closed Then
                    cnConnection.Open()
                End If
                cmdCommand.CommandText = SearchQuery
                cmdCommand.Connection = cnConnection
                Return cmdCommand.ExecuteScalar
            End Using
        Catch ex As Exception
            CreateLog("clsData", "SearchSingleValue-519", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Closes the DBConnection object only for DBDataReader
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CloseConnection()
        If Not (cnConnection.State = System.Data.ConnectionState.Closed) Then
            cnConnection.Close()
        End If
    End Sub

    ''' <summary>
    ''' Return the state of the DBConnection object only for DBDataReader
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsConnected() As Boolean
        If Not (cnConnection.State = System.Data.ConnectionState.Closed) Then
            Return True
        Else
            Return False
        End If
    End Function
    ''' <summary>
    ''' This Function will be used to execute a SP with Two Parameter
    ''' </summary>
    Public Shared Sub ExecuteSPwithTwoParameters(ByVal procedurename As String, ByVal parameter As String, ByVal screenName As String)
        Using cnConnection As System.Data.Common.DbConnection = Connection, _
                           cmdCommand As System.Data.SqlClient.SqlCommand = Command
            Try
                cnConnection.ConnectionString = ConnectionString
                cmdCommand.Connection = cnConnection
                cmdCommand.CommandText = procedurename
                cnConnection.Open()
                cmdCommand.CommandType = CommandType.StoredProcedure
                cmdCommand.Parameters.AddWithValue("@parameter", parameter)
                cmdCommand.Parameters.AddWithValue("@ScreenName", screenName)
                cmdCommand.ExecuteNonQuery()
            Catch ex As Exception
                CreateLog("clsData", "ExecuteSPwithOneParameter-684", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Throw ex
            Finally
                cnConnection.Close()
            End Try
        End Using
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Cmd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Search(ByVal Cmd As System.Data.SqlClient.SqlCommand) As System.Data.DataTable

        Using cnConnection As System.Data.Common.DbConnection = Connection
            'adAdapter As System.Data.SqlClient.SqlDataAdapter = DataAdapter
            Dim dtresult As New DataTable
            Try
                cnConnection.ConnectionString = ConnectionString
                Cmd.Connection = cnConnection
                Cmd.CommandTimeout = 0
                sqADP = New SqlDataAdapter()
                sqADP.SelectCommand = Cmd
                sqADP.Fill(dtresult)

            Catch ex As Exception
                sqADP.Dispose()
                dsResults.Dispose()
                CreateLog("clsData", "Search-876", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

            End Try
            Return dtresult
        End Using
    End Function
    Public Shared Function SearchDSPro(ByVal Cmd As System.Data.SqlClient.SqlCommand) As System.Data.DataSet

        Using cnConnection As System.Data.Common.DbConnection = Connection
            'adAdapter As System.Data.SqlClient.SqlDataAdapter = DataAdapter
            Dim dsResult As New DataSet
            Try
                cnConnection.ConnectionString = ConnectionString
                Cmd.Connection = cnConnection
                sqADP = New SqlDataAdapter()
                sqADP.SelectCommand = Cmd
                sqADP.Fill(dsResult)

            Catch ex As Exception
                sqADP.Dispose()
                dsResults.Dispose()
                CreateLog("clsData", "Search-876", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)

            End Try
            Return dsResult
        End Using
    End Function

    Public Shared Function ExecuteProcedure(ByVal Cmd As System.Data.Common.DbCommand) As Boolean

        Using cnConnection As System.Data.Common.DbConnection = Connection
            Try
                cnConnection.ConnectionString = ConnectionString
                Cmd.Connection = cnConnection
                cnConnection.Open()
                Cmd.ExecuteNonQuery()
                Return True
            Catch ex As Exception
                CreateLog("clsData", "ExecuteProcedure-778", LogType.Application, LogSubType.Exception, ex.TargetSite.Attributes, ex.ToString)
                Return False
            End Try
        End Using
    End Function


#Region " Private properties "

    Private Shared ReadOnly Property Command() As System.Data.Common.DbCommand
        Get
            Return Factory.CreateCommand()
        End Get
    End Property

    Private Shared ReadOnly Property DataAdapter() As System.Data.Common.DbDataAdapter
        Get
            Return Factory.CreateDataAdapter
        End Get
    End Property

    Private Shared ReadOnly Property Connection() As System.Data.Common.DbConnection
        Get
            Return Factory.CreateConnection
        End Get
    End Property

    Private Shared ReadOnly Property CommandBuilder() As System.Data.Common.DbCommandBuilder
        Get
            Return Factory.CreateCommandBuilder
        End Get
    End Property
#End Region

End Class