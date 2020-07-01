# Sello E-Commerce Platform - Running Logic Apps Anywhere

## Scenario
Sello provides an e-commerce platform allowing their customers to purchase products from the catalog and initiate shipments to their homes.

They are planning on going to the cloud but unfortunately depend on legacy systems, such as a SQL Server database, and decided to use a multi-phase migration.

As part of their application modernization, Sello wants to run cloud-native application on-premises to be ready when they migrate to the cloud. By packaging their apps as containers and run them on Kubernetes on-premises in order to connect to their legacy systems such as SQL Server.

Their landscape contains the following components:
-	An **Order service** where customers can create an order
    - This exposes a REST endpoint which is served by a .NET Core Web API. Every order request will check the warehouse service to see if the order is available and initiate a shipment
-	A **Warehouse service** to manage the stock of products that are available
    - Provides REST endpoint to verify stock of a product and update it once shipments are initiated. All stock information is stored in an on-premises SQL database.
-	A **Shipment service** to initiate shipments for orders
    - Provides a REST endpoint to accept new shipment requests which will be handled asynchronously
