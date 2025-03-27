## Introduction

**Description**:  
WarehouseProject is a warehouse management application that integrates Gemini AI to support intelligent chatbots and Cloudinary for image management.  
The project provides features such as product, order, customer, and supplier management, with a friendly user interface that allows users to chat with the chatbot to query system information.  
WarehouseProject is designed to ensure that the chatbot only responds based on internal data, providing an efficient and secure warehouse management experience.

---

## Usage

• In WarehouseProject, right-click on WarehouseProject in Solution Explorer.  
• Select Manage User Secrets from the context menu.  
• Visual Studio will automatically create User Secrets for the project if it does not already exist. This will add a UserSecretsId to the WarehouseProject.csproj file.  
After opening `secrets.json`, you can add the following information:

```json
{
  "Gemini": {
    "ApiKey": "your-gemini-api-key"
  },
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-cloud-api-key",
    "ApiSecret": "your-cloud-api-secret"
  }
}
