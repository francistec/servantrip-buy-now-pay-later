# BNPL Microservice – ServanTrip (ASP.NET Core 8 + Stripe)

Este proyecto implementa un **microservicio BNPL (“Buy Now, Pay Later”)** para ServanTrip, totalmente desacoplado de la plataforma principal y construido con:

- **.NET 8 Web API**
- **Arquitectura Limpia (Domain + Application + Infrastructure + API)**
- **Stripe Subscription Schedules**
- **Políticas dinámicas de cobro**
- **Modo Debug para pruebas inmediatas**

El microservicio calcula fechas de pago diferido, programa cargos futuros en Stripe y notifica mediante webhooks.

---

## 🚀 Características principales

### ✔ Microservicio independiente
No depende de los flujos actuales de ServanTrip.  
Se puede desplegar en cualquier entorno aislado.

### ✔ Cálculo inteligente BNPL
- Traslados → 48 horas antes  
- Actividades → 72 horas antes  
- Debug → cobra en el acto (solo para pruebas)

### ✔ Cargos futuros automáticos con Stripe
Usa **Subscription Schedules** con:
- 1 sola phase  
- 1 invoice  
- Cobro automático en `ChargeDate`  
- Schedule se cancela solo al finalizar  

### ✔ Programación de activación
Stripe activa el schedule en:

StripeActivationDate = ChargeDate - 15 días


### ✔ Reintentos automáticos
Stripe reintenta según la configuración de Billing.


### ✔ API REST documentada con Swagger
`POST /api/bookings`

---

## Arquitectura del proyecto

```text
BNPL.sln
│
├── BNPL.Api → Endpoints REST, DI, Swagger
│
├── BNPL.Application → DTOs, BookingService, lógica de orquestación
│
├── BNPL.Domain → Policies, Entities, Enums, PaymentCalculator
│
├── BNPL.Infrastructure → Stripe integration (SubscriptionSchedules)
│
└── BNPL.Notifications → (pendiente) Emails transaccionales
```

---

## 📦 Dominio (Domain Layer)

### **Entities**
- `Booking`
- `PaymentSchedule`
- `BnplSchedule`

### **Enums**
- `ProductType` (`Transfer`, `Activity`, `Debug`)
- `PaymentStatus`

### **Policies**
- `TransferPolicy` (cobro 48h antes)
- `ActivityPolicy` (cobro 72h antes)
- `DebugPolicy` (cobro inmediato)

### **Services**
- `PaymentCalculator`  
  Calcula:  
  - `ChargeDate`  
  - `StripeActivationDate`  
  - `AmountToCharge` (30% por ahora)

---

## 🔧 Integración con Stripe (Infrastructure)

Implementada en:

📄 `BNPL.Infrastructure/Stripe/PaymentSchedulerService.cs`

### Funciona así:

- Crea Customer en Stripe  
- Crea InvoiceItem con monto real  
- Crea Price dummy ($0) para cumplir requisitos de Stripe  
- Crea SubscriptionSchedule con:
  - `start_date = StripeActivationDate`
  - `phase.end_date = ChargeDate`
  - `billing_cycle_anchor = "automatic"`
  - `end_behavior = "cancel"`

### Resultado:
✔ Stripe cobra automáticamente en ChargeDate  
✔ Schedule se cancela sola  
✔ Se genera 1 invoice  
✔ Reintentos automáticos  
✔ Cliente ve un cargo normal  

---

## 🌐 Endpoint principal

### `POST /api/bookings`

**Request**

```json
{
  "bookingId": "BNPL_001",
  "productType": "Transfer",
  "totalAmount": 1500,
  "serviceDate": "2026-02-15T00:00:00",
  "userEmail": "cliente@example.com"
}
```
**Response**
```json
{
  "bookingId": "BNPL_001",
  "amountToCharge": 450,
  "scheduledChargeDate": "2026-02-13T00:00:00Z",
  "status": "scheduled",
  "stripeScheduleId": "sub_sched_1Qxyz..."
}
```

## 📘 Próximos pasos (pendientes)

- [ ] Webhooks Stripe:
    - [ ] invoice.paid
    - [ ] invoice.payment_failed
    - [ ] subscription_schedule.canceled
    - [ ] customer.subscription.deleted
 - [ ] Guardar BNPL schedule en base de datos
 - [ ] Emails transaccionales (programado, recordatorio, pagado, fallido)
 - [ ] Fallback PaymentIntent
 - [ ] Logs estructurados (Serilog)
 - [ ] Unit Tests
 - [ ] Implementación de reintentos personalizados

 ## 🏁 Estado actual

✔ Arquitectura base lista
✔ Stripe SubscriptionSchedule creado correctamente
✔ Endpoint funcional en Swagger
✔ Modo Debug agregado
✔ Código modular, limpio y extensible