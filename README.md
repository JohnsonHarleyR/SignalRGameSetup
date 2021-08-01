You will want to add a connectionstrings.config file with your database information.
It should follow this format:

<connectionStrings>
  <add name="SignalRGame" connectionString="YourConnectionStringHere" providerName="System.Data.SqlClient" />
</connectionStrings>

---------------------------------------------------------------------------------------------------
For whichever game you choose to add, the basic information should be entered into this class:

Logic > GameInformation
