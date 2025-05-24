# ⏱️ FunctionUptime – Monitor Website and API Uptime with Azure Functions and Logic Apps

A simple, serverless uptime monitoring tool built with **Azure Functions**, **Logic Apps**, and **.NET**.  
This project checks the availability of a website or API on a schedule and sends an email alert if it's down or returns an error status.

> ✅ Ideal for .NET developers looking to explore serverless workflows in Azure using the free tier.

---

## 🚀 Features

- Check uptime of any HTTP(S) endpoint
- Schedule checks using CRON expressions
- Trigger email alerts via Logic Apps (no SendGrid required)
- Built entirely in Azure – no local deployment needed
- Clean and beginner-friendly .NET C# implementation

---

## 🛠️ Technologies Used

- Azure Functions (.NET Isolated Worker)
- Azure Logic Apps (Consumption Plan)
- Visual Studio Code
- C# / .NET 6+
- Azurite (for local testing)
- Azure Free Tier

---

## 📦 Project Structure
FunctionUptime/
├── CheckWebsiteUptime.cs # Azure Function logic
├── local.settings.json # Local dev settings (excluded from deployment)
├── host.json / function.json # Azure config files
└── README.md # Project documentation

## ⚙️ Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/ottorinobruni/FunctionUptime.git
cd FunctionUptime
