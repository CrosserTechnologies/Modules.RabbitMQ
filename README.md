# Modules.RabbitMQ

Crosser modules for communication with RabbitMQ

---

Note that you currently need to add a `nuget.config` to crossers internal nuget server if you want to be able to build & run the tests.
This is because we currently reference a preview package of `Crosser.EdgeNode.Modules.Testing`. As soon as the package is available on the official nuget this will no longer be required.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
 <packageSources>
    <add key="nuget-internal" value="https://path-to-nuget-server/v3/index.json" />
  </packageSources>
</configuration>
```
