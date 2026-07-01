<!--Agregar

namespace SNE_FacturasValidadas_XML.src.views
{
    public class ValidadorFacturas
    {

<script setup>
    import { ref } from 'vue'

    const archivo = ref(null)
    const cargando = ref(false)
    const resultado = ref(null)
    const error = ref('')

    const seleccionarArchivo = (event) => {
        archivo.value = event.target.files[0]
    }

    const validarFactura = async () => {

        error.value = ''
        resultado.value = null

        if (!archivo.value) {
            error.value = 'Selecciona un archivo XML'
            return
        }

        try {

            cargando.value = true

            const formData = new FormData()

            formData.append(
                'archivo',
                archivo.value
            )

            const response = await fetch(
                'https://localhost:7068/api/facturas/validar',
                {
                    method: 'POST',
                    body: formData
                }
            )

            if (!response.ok) {
                throw new Error(
                    'Error al validar factura'
                )
            }

            resultado.value =
                await response.json()

        }
        catch (err) {

            error.value = err.message

        }
        finally {

            cargando.value = false

        }
    }
</script>

<template>

    <div class="min-h-screen flex items-center justify-center bg-gray-100">

        <div class="bg-white rounded-xl shadow-lg p-8 w-full max-w-2xl">

            <h1 class="text-2xl font-bold mb-6">
                Validador CFDI SAT
            </h1>

            <input type="file"
                   accept=".xml"
                   @change="seleccionarArchivo"
                   class="mb-4 w-full border rounded p-2">

            <button @click="validarFactura"
                    :disabled="cargando"
                    class="bg-blue-600 text-white px-4 py-2 rounded">
                {{ cargando ? 'Validando...' : 'Validar XML' }}
            </button>

            <div v-if="error"
                 class="mt-4 text-red-500">
                {{ error }}
            </div>

            <div v-if="resultado"
                 class="mt-6 border rounded p-4">

                <h2 class="font-bold text-lg mb-3">
                    Resultado
                </h2>

                <p>
                    <strong>UUID:</strong>
                    {{ resultado.uuid }}
                </p>

                <p>
                    <strong>RFC Emisor:</strong>
                    {{ resultado.rfcEmisor }}
                </p>

                <p>
                    <strong>RFC Receptor:</strong>
                    {{ resultado.rfcReceptor }}
                </p>

                <p>
                    <strong>Total:</strong>
                    ${{ resultado.total }}
                </p>

                <p>
                    <strong>Estado SAT:</strong>
                    {{ resultado.estadoSAT }}
                </p>

            </div>

        </div>

    </div>

</template>

    }
}
 import ValidadorFacturas
from '@/views/ValidadorFacturas.vue'

{
    path: '/facturas',
    component: ValidadorFacturas
}-->
